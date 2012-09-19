using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.ServiceModel;
using Procesta.CvServer.Class.Methods;
using System.Data.SqlClient;
using System.Windows.Threading;
namespace Procesta.CvServer
{
	/// <summary>
	/// Interaction logic for StratWindow.xaml
	/// </summary>
	public partial class StratWindow : Window,INotifyPropertyChanged
	{
        private BackgroundWorker startBackgroundWorker = new BackgroundWorker();
        private MainWindow serverWindow;
        /// <summary>
        /// Window instillation  
        /// </summary>
		public StratWindow()
		{
			this.InitializeComponent();
            startBackgroundWorker.WorkerReportsProgress = true;
            startBackgroundWorker.DoWork += new DoWorkEventHandler(startBackgroundWorker_DoWork);
            startBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(startBackgroundWorker_RunWorkerCompleted);
            startBackgroundWorker.RunWorkerAsync();
		}
        
        /// <summary>
        ///BackGroundWorker Work Complete  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void startBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error==null)
            {
               // new MainWindow().Show();
                if (serverWindow!=null)
                {
                    serverWindow.Show();
                }
                this.Close();
            }
            else
            {
                /// Code here to write log file
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// BackGroundWorker Do Work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void startBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.UpperText = "Connecting";
            this.LowerText = "Database";
            Thread.Sleep(1000);
            using (SqlConnection testConnection = new SqlConnection(Properties.Settings.Default.Cafeteria_Vernier_dbConnectionString))
            {
                try
                {
                    testConnection.Open();
                }
                catch
                {
                    this.UpperText = "Connection";
                    this.LowerText = "Fault";
                    Thread.Sleep(1000);
                    new InstallWindow().Show();
                    this.Close();
                    return;
                }
            }
            this.UpperText = "Connecting";
            this.LowerText = "Service";
            Thread.Sleep(2000);
            //Check connection Named pipe service
            this.UpperText = "Retrieving";
            this.LowerText = "Necessary Information";
            Thread.Sleep(2000); 
            MiraculousMethods miraculousMethod = new MiraculousMethods();
            miraculousMethod.CheckCashDate();
            this.Dispatcher.BeginInvoke(new Action(()=>
            {
                serverWindow = new MainWindow();
               
                serverWindow.BillConfigInfo =  miraculousMethod.LoadBillConfig();
            }),DispatcherPriority.Normal); 
            
            // 
            //
            //miraculousMethod.MinimumRequirement();
        }

        /// <summary>
        /// Start WCF Server at 9078 Port.
        /// It run another thread 
        /// </summary>
        private static void startWcfServer()
        {
            try
            {
                //ServiceHost duplex = new ServiceHost(typeof(ConnectFromServer));
                //NetTcpBinding tcpBinding = new NetTcpBinding();
                //tcpBinding.Security.Mode = SecurityMode.None;
                //duplex.AddServiceEndpoint(typeof(IServerWithCallback), tcpBinding, string.Format("net.tcp://{0}:9078/DataService", Properties.Settings.Default.ServerIpAddress));
                //duplex.Open();
            }
            catch { }
        }

        #region Animation Text Property
        public string UpperText
        {
            get { return this._UpperText; }
            set
            { 
                this._UpperText = value;
                this.onPropertyChange("UpperText");
            }
        }
        public string LowerText
        {
            get { return this._LowerText; }
            set
            {
                this._LowerText = value;
                this.onPropertyChange("LowerText");
            }
        }
        private string _UpperText;
        private string _LowerText;
        #endregion

        #region Property Chnage
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChange(string propertName)
        {
            if (this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertName));
            }
        }
        #endregion
    }
}