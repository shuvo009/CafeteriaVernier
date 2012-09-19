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
using System.Windows.Shapes;
using System.Net;
using DevExpress.Xpf.Core;
using Procesta.CvServer.Class;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Diagnostics;
using System.Data.SqlClient;
namespace Procesta.CvServer
{
    /// <summary>
    /// Interaction logic for InstallWindow.xaml
    /// </summary>
    public partial class InstallWindow : Window
    {
        public InstallWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Save Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpaddressButtonSaveClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.IpaddressButtonSave.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            bool isDatabaseFound = false;
            try
            {
                if (this.IpAddressComBoBox.Text != string.Empty && !String.IsNullOrWhiteSpace(this.IpAddressComBoBox.Text))
                {
                    Properties.Settings.Default.IsConfigered = true;
                    Server databaseServer = new Server(new ServerConnection(CvVariables.SQL_SERVER_NAME));
                    foreach (Database serverDatabase in databaseServer.Databases)
                    {
                        if (serverDatabase.Name.Equals(CvVariables.Catalog))
                        {
                            isDatabaseFound = true;
                        }
                    }
                    if (isDatabaseFound)
                    {
                        Mouse.OverrideCursor = null;
                        MessageBoxResult messBoxResult = DXMessageBox.Show(CvVariables.ERROR_MESSAGES[2, 7], CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.YesNo, MessageBoxImage.Question);
                        Mouse.OverrideCursor = Cursors.Wait;
                        if (messBoxResult.Equals(MessageBoxResult.Yes))
                        {
                            this.RestoreDatabase();
                        }

                    }
                    else
                    {
                        this.RestoreDatabase();
                    }
                    Properties.Settings.Default.Save();
                    System.Windows.Forms.Application.Restart();
                    System.Windows.Application.Current.Shutdown(); 
                    this.Close();
                }
                else
                {
                     DXMessageBox.Show(CvVariables.ERROR_MESSAGES[0, 1],CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.IpaddressButtonSave.IsEnabled = true;
            }
        }
        /// <summary>
        /// Window Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpAddressWindowsLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IPAddress[] localPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress WorkingIp in localPs)
            {
                this.IpAddressComBoBox.Items.Add(WorkingIp);
            }
        }
        /// <summary>
        /// Restore Database at First time Setup
        /// </summary>
        private void RestoreDatabase()
        {
            try
            {
                using (SqlConnection restoreConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True"))
                {
                    restoreConnection.Open();
                    SqlCommand UseMasterCommand = new SqlCommand("USE master", restoreConnection);
                    UseMasterCommand.ExecuteNonQuery();
                    string Restore = string.Format(@"RESTORE DATABASE [{0}] FROM DISK = N'{1}' WITH  FILE = 1,  NOUNLOAD,  STATS = 10,MOVE '{0}' TO " + @"'{2}\{0}.mdf',MOVE '{0}_log' TO '{2}\{0}_log.ldf'", CvVariables.Catalog, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "cvdb.bak"), Properties.Settings.Default.SqlDataFolder);
                    SqlCommand RestoreCmd = new SqlCommand(Restore, restoreConnection);
                    RestoreCmd.ExecuteNonQuery();
                    string Alter2 =string.Format(@"ALTER DATABASE [{0}] SET Multi_User",CvVariables.Catalog);
                    SqlCommand Alter2Cmd = new SqlCommand(Alter2, restoreConnection);
                    Alter2Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }
        /// <summary>
        /// Brows Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configButtonBrowsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.configButtonBrows.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait; ;
            try
            {
                System.Windows.Forms.FolderBrowserDialog backupPathDialog = new System.Windows.Forms.FolderBrowserDialog();
                backupPathDialog.Description = "Select SQl Server DATA Folder";
                backupPathDialog.ShowNewFolderButton = false;
                if (backupPathDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
                {
                    this.ConfigDataFolderPath.Text = backupPathDialog.SelectedPath;
                    Properties.Settings.Default.SqlDataFolder=backupPathDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.configButtonBrows.IsEnabled = true;
            }
        }
    }
}
