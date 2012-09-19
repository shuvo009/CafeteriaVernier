using System;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;
using System.Net;
using System.Threading;
using Procesta.CVClient.Class.Methords;
using Procesta.CVClient.Class.CvCPropertes;
using Procesta.CVClient.ServerSideService;
using Microsoft.Win32;
using DevExpress.Xpf.Core;
using Procesta.CVClient.Class;
namespace Procesta.CVClient
{
    /// <summary>
    /// Interaction logic for StratWindow.xaml
    /// </summary>
    public partial class StratWindow : Window, INotifyPropertyChanged
    {
        private BackgroundWorker startBackgroundWorker = new BackgroundWorker();
        private string _Messages;
        public string Messages
        {
            get
            {
                return this._Messages;
            }
            set
            {
                this._Messages = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Messages"));
                }
            }
        }

        /// <summary>
        /// Window instillation  
        /// </summary>
        public StratWindow()
        {
            this.InitializeComponent();
            startBackgroundWorker.DoWork += new DoWorkEventHandler(startBackgroundWorker_DoWork);
            startBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(startBackgroundWorker_RunWorkerCompleted);
            //if (!Properties.Settings.Default.IsConfigered)
            //{
            //    new InstallWindow().Show();
            //    this.Close();
            //    return;
            //}
            startBackgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        ///BackGroundWorker Work Complete  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void startBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                DXMessageBox.Show(e.Error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        /// <summary>
        /// BackGroundWorker Do Work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void startBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Messages = "Checking...";
                this.startupEntry();
                Thread.Sleep(1000);
                this.Messages = "Service 1";
                ServerConnection.NotifactionToServer();
                Thread.Sleep(1000);
                this.Messages = "Service 2";
                ServerConnection.NotifactionFromServer();
                Thread.Sleep(1000);
                this.Messages = "Service 3";
                ServerConnection.ConnectToService();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Registry Data Entry for startup
        /// </summary>
        private void startupEntry()
        {
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (regKey != null)
                {
                    object keyValue = regKey.GetValue("CvClient");
                    if (keyValue == null)
                    {
                        Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true).SetValue("CVClient", System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}