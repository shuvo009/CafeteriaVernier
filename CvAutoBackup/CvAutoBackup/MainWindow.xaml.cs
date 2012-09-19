using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.ComponentModel;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
namespace CvAutoBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private DispatcherTimer timeDecrement = new DispatcherTimer();
        private TimeSpan timeDec = new TimeSpan();
        private string _imagePath;
        private System.Threading.Thread autoBackupThread;
        private System.Windows.Forms.NotifyIcon cvAutoBackupNotify;
        public string ImagePath
        {
            get { return this._imagePath; }
            set { this._imagePath = value; this.OnPropertyChange("ImagePath"); }
        }

        public MainWindow()
        {
            InitializeComponent();
            cvAutoBackupNotify = new System.Windows.Forms.NotifyIcon();
            cvAutoBackupNotify.Icon = Properties.Resources.cvAutobackupIcon;
            cvAutoBackupNotify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(cvAutoBackupNotify_MouseDoubleClick);
            timeDecrement.Tick += new EventHandler(timeDecrement_Tick);
            List<timeSchedule> scheduleList = new List<timeSchedule>();
            scheduleList.Add(new timeSchedule { time = "20 Min", value = new TimeSpan(0, 20, 0) });
            scheduleList.Add(new timeSchedule { time = "30 Min", value = new TimeSpan(0, 30, 0) });
            scheduleList.Add(new timeSchedule { time = "1 Hour", value = new TimeSpan(1, 0, 0) });
            scheduleList.Add(new timeSchedule { time = "2 Hour", value = new TimeSpan(2, 0, 0) });
            scheduleList.Add(new timeSchedule { time = "3 Hour", value = new TimeSpan(3, 0, 0) });
            scheduleList.Add(new timeSchedule { time = "4 Hour", value = new TimeSpan(4, 0, 0) });
            scheduleList.Add(new timeSchedule { time = "5 Hour", value = new TimeSpan(5, 0, 0) });
            backupAfterCombo.ItemsSource = scheduleList;
            timeDec = Properties.Settings.Default.backupAfter;
            timeDecrement.Interval = new TimeSpan(0, 0, 1);
            timeDecrement.Start();
            ImagePath = "database.png";
            this.lastbackupTime.Text = Properties.Settings.Default.lastBackup.ToString();
            autoBackupThread = new System.Threading.Thread(() => this.autoBackUp());
            autoBackupThread.Start();
        }

        void cvAutoBackupNotify_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        void timeDecrement_Tick(object sender, EventArgs e)
        {
            this.timeLeftTxt.Text = timeDec.ToString();
            if (timeDec.Equals(new TimeSpan(0,0,0)))
            {
                this.timeDecrement.Stop();
                this.backupProgress.IsIndeterminate = true;
                this.ImagePath = "dbbackup.png";
            }
            else
            {
                timeDec = timeDec + new TimeSpan(0, 0, -1);
            }
        }

        #region MainWindow 
        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           MessageBoxResult messResult= MessageBox.Show(Variables.ERROR_MESSAGES[1],Variables.ERROR_MESSAGES[0],MessageBoxButton.YesNo,MessageBoxImage.Question);
           if (messResult.Equals(MessageBoxResult.Yes))
           {
               e.Cancel = false;
               if (autoBackupThread.IsAlive)
               {
                   autoBackupThread.Abort();
               }
               
           }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.backupPath) || Properties.Settings.Default.backupPath==string.Empty)
            {
                Properties.Settings.Default.backupPath = folderCreateor(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CVBackup");
                Properties.Settings.Default.Save();
            }
            this.backupPathText.Text = Properties.Settings.Default.backupPath;
            this.backupAfterCombo.Text = Properties.Settings.Default.backupTime;
        }
        #endregion

        private void browsPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog savePath = new System.Windows.Forms.FolderBrowserDialog();
            savePath.Description = "Select the directory that you want to use as the default.";
            savePath.ShowNewFolderButton = true;
            savePath.RootFolder = Environment.SpecialFolder.Desktop;
            System.Windows.Forms.DialogResult folderResult =savePath.ShowDialog();
            if (folderResult.Equals(System.Windows.Forms.DialogResult.OK))
            {
                Properties.Settings.Default.backupPath = savePath.SelectedPath;
                this.backupPathText.Text=savePath.SelectedPath;
            }
        }

        private string folderCreateor(string folderPath, string folderName)
        {
            string fullPath = System.IO.Path.Combine(folderPath, folderName);
            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Variables.ERROR_MESSAGES[0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return fullPath;
        }

        private void saveSetting_Click(object sender, RoutedEventArgs e)
        {
            this.saveSetting.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (this.backupAfterCombo.Text!=string.Empty)
                {
                    timeSchedule tempSchedule = this.backupAfterCombo.SelectedItem as timeSchedule;
                    Properties.Settings.Default.backupAfter = tempSchedule.value;
                    Properties.Settings.Default.backupTime = tempSchedule.time;
                    Properties.Settings.Default.Save();
                    timeDec = tempSchedule.value;
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(Variables.ERROR_MESSAGES[3], Variables.ERROR_MESSAGES[0], MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    throw new Exception(Variables.ERROR_MESSAGES[2]);
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(error.Message, Variables.ERROR_MESSAGES[0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.saveSetting.IsEnabled = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string propertyName)
        {
            if (this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void autoBackUp()
        {
            for (; ; )
            {
                try
                {
                    using (SqlConnection backupConnection = new SqlConnection(Properties.Settings.Default.bdConnectionString))
                    {
                        backupConnection.Open();
                        ServerConnection sc = new ServerConnection(backupConnection);
                        Server databaseServer = new Server(sc);
                        Backup databaseBackup = new Backup();
                        databaseBackup.Action = BackupActionType.Database;
                        databaseBackup.Database = backupConnection.Database.ToString();
                        databaseBackup.Devices.Add(new BackupDeviceItem(System.IO.Path.Combine(this.folderCreateor(Properties.Settings.Default.backupPath, DateTime.Today.ToString("yyyy-MM-dd")), string.Format("{0}.bak", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-tt"))), DeviceType.File));
                        databaseBackup.LogTruncation = BackupTruncateLogType.Truncate;
                        databaseBackup.SqlBackup(databaseServer);
                        sc.ForceDisconnected();
                        databaseBackup = null;
                        backupConnection.Dispose();
                    }
                    timeDec = Properties.Settings.Default.backupAfter;
                    Properties.Settings.Default.lastBackup = DateTime.Now;
                    ImagePath = "database.png";
                    Action updateInfo = new Action(() => { this.backupProgress.IsIndeterminate = false; this.lastbackupTime.Text = DateTime.Now.ToString(); this.timeDecrement.Start(); });
                    this.Dispatcher.Invoke(updateInfo, DispatcherPriority.Normal);
                    System.Threading.Thread.Sleep(Properties.Settings.Default.backupAfter);

                }
                catch (System.Threading.ThreadAbortException)
                {
                    
                }
                catch (FailedOperationException ex)
                {
                    Action errorUpdate = new Action(() => { MessageBox.Show(string.Format("{0}\n{1}",ex.Message,Variables.ERROR_MESSAGES[4]), Variables.ERROR_MESSAGES[0], MessageBoxButton.OK, MessageBoxImage.Error); });
                    this.Dispatcher.Invoke(errorUpdate, DispatcherPriority.Normal);
                    System.Threading.Thread.Sleep(Properties.Settings.Default.backupAfter);
                }
                catch (Exception ex)
                {
                    Action errorUpdate = new Action(() => { MessageBox.Show(ex.Message, Variables.ERROR_MESSAGES[0], MessageBoxButton.OK, MessageBoxImage.Error); });
                    this.Dispatcher.Invoke(errorUpdate, DispatcherPriority.Normal);
                    System.Threading.Thread.Sleep(Properties.Settings.Default.backupAfter);
                }
            }
        }

        private void MianWindowStateChanged(object sender, System.EventArgs e)
        {
            if (this.WindowState.Equals(WindowState.Minimized))
            {
                this.ShowInTaskbar = false;
                cvAutoBackupNotify.BalloonTipTitle = Variables.ERROR_MESSAGES[0];
                cvAutoBackupNotify.BalloonTipText = Variables.ERROR_MESSAGES[5];
                cvAutoBackupNotify.ShowBalloonTip(400);
                cvAutoBackupNotify.Visible = true;
            }
            else
            {
                cvAutoBackupNotify.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        public static void sysTryStart()
        {
            MainWindow systrayMainWindow = new MainWindow();
            
        }
    }

    public class timeSchedule
    {
        public string time { get; set; }
        public TimeSpan value { get; set; }
    }
}
