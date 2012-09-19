using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Procesta.CVClient.Class.CvCPropertes;
using System.Windows.Threading;
using Procesta.CVClient.Class.Methords;
using DevExpress.Xpf.Core;
using Procesta.CVClient.Class;
using System.ServiceModel;
using Procesta.CVClient.ServerNotifaction;
using System.ComponentModel;
namespace Procesta.CVClient
{
	/// <summary>
	/// Interaction logic for Counter.xaml
	/// </summary>
    public partial class Counter : Window, INotifyPropertyChanged
    {
        #region Property
        private UserInfoViewer _customerDetail;
        public UserInfoViewer customerDetail
        {
            get
            {
                return this._customerDetail;
            }
            set
            {
                this._customerDetail = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("customerDetail"));
                }
            }
        }
        public static Window counterWindow = null;
        #endregion

        #region Private Variables
        DispatcherTimer timeCounter = new DispatcherTimer();
        short counterNumber;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Counter()
		{
            customerDetail = new UserInfoViewer();
			this.InitializeComponent();
            this.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - (this.Height + 50);
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - (this.Width + 10);
            timeCounter.Interval = new TimeSpan(0, 0, 59);
            counterNumber = Properties.Settings.Default.CounterNumber;
            timeCounter.Start();
            this.Closing += new System.ComponentModel.CancelEventHandler(Counter_Closing);
            counterWindow = this;
		}

        void Counter_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.timeCounter.Stop();
            new MainWindow().Show();
            this.LogoutOperation();
            MiraculousMethods.conterInformation.CustomerID = null;
            MiraculousMethods.conterInformation.Status = CounterStatus.Free;
            customerDetail = null;
        }
        /// <summary>
        /// With User Information
        /// </summary>
        /// <param name="userinfo"></param>
        public static void CounterWindow(UserInfoViewer loggedInUserInfo)
        {
            Counter counterWindow = new Counter();
            counterWindow.customerDetail = loggedInUserInfo;
            MiraculousMethods.conterInformation.CustomerID = loggedInUserInfo.Username;
            MiraculousMethods.conterInformation.Status = CounterStatus.Busy;
            if (String.IsNullOrEmpty(loggedInUserInfo.TeamName) || String.IsNullOrWhiteSpace(loggedInUserInfo.TeamName))
            {
                counterWindow.userLoginTimer();
            }
            else
            {
                counterWindow.teamLoginTimer();
            }
            counterWindow.Show();
        }
        #endregion

        #region Timer Handelar
        /// <summary>
        /// User munities decrement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserTimeDecriment(object sender, EventArgs e)
        {
            ConnectionChecker();
            if (ServerConnection.serviceFromServer.UserMunitieCounter(customerDetail.Username, counterNumber))
            {
                customerDetail.Minutes = ServerConnection.serviceFromServer.GetUserBalance(customerDetail.Username);
            }
            else
            {
                customerDetail.Minutes = ServerConnection.serviceFromServer.GetUserBalance(customerDetail.Username);
                this.TimeChecker();
            }
            
        }
        /// <summary>
        /// Team munities decrement 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeamTimeDecriment(object sender, EventArgs e)
        {
            ConnectionChecker();
            if (ServerConnection.serviceFromServer.TeamMunitieCounter(customerDetail.TeamName, customerDetail.Username, counterNumber))
            {
                customerDetail.Minutes = ServerConnection.serviceFromServer.GetTeamBalance(customerDetail.TeamName);
            }
            else
            {
                customerDetail.Minutes = ServerConnection.serviceFromServer.GetTeamBalance(customerDetail.TeamName);
                this.TimeChecker();
            }
            
        }
        #endregion

        #region Buttons
        public ICommand LogoutCommand
        {
            get { return new ReplayCommand(new Action<object>(this.logout)); }
        }

        public ICommand SettingCommand
        {
            get { return new ReplayCommand(new Action<object>(this.setting)); }
        }

        private void logout(object obj)
        {
            this.btnLogout.IsEnabled = false;
            try
            {
                if (DXMessageBox.Show(CVsVariables.ERROR_MESSAGES[0, 3], CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.YesNo, MessageBoxImage.Question).Equals(MessageBoxResult.Yes))
                {
                    //Mouse.OverrideCursor = Cursors.Wait;
                    //this.timeCounter.Stop();
                    //new MainWindow().Show();
                    //this.LogoutOperation();
                    //customerDetail = null;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ex.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                Mouse.OverrideCursor = null;
                this.btnLogout.IsEnabled = true;
            }
        }

        private void setting(object obj)
        {
            this.btnSetting.IsEnabled = false;
            try
            {
                Settings.counter = this;
                new Settings().ShowDialog();
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.btnSetting.IsEnabled = true;
            }
        }
        #endregion

        #region Private Mentod
        /// <summary>
        /// Check Time is >1 or not
        /// </summary>
        private void TimeChecker()
        {
            if (customerDetail.Minutes <= 1)
            {
                this.timeCounter.Stop();
                new TimeOut().Show();
                this.LogoutOperation();
                customerDetail = null;
                this.Close();
            }
        }
        /// <summary>
        /// Timer Handle change for user login
        /// </summary>
        private void userLoginTimer()
        {
            timeCounter.Tick -= new EventHandler(TeamTimeDecriment);
            timeCounter.Tick += new EventHandler(UserTimeDecriment);
        }
        /// <summary>
        /// Timer handle change for team login
        /// </summary>
        private void teamLoginTimer()
        {
            timeCounter.Tick += new EventHandler(TeamTimeDecriment);
            timeCounter.Tick -= new EventHandler(UserTimeDecriment);
        }
        /// <summary>
        /// Logout Operation
        /// </summary>
        private void LogoutOperation()
        {
            try
            {
                this.ConnectionChecker();
                if (customerDetail.TeamName != string.Empty && customerDetail.TeamName != null)
                {
                    ServerConnection.serviceFromServer.TeamLogout(username: customerDetail.Username, teamName: customerDetail.TeamName, loginHistoryID: customerDetail.LoginHistoryID);
                }
                else
                {
                    ServerConnection.serviceFromServer.UserLogout(customerDetail.Username,customerDetail.LoginHistoryID);
                }
            }
            catch (Exception error)
            {
                DXMessageBox.Show(error.Message, CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Check Server Connection Is Alive or not
        /// </summary>
        private void ConnectionChecker()
        {
            if (ServerConnection.serviceFromServer == null || !ServerConnection.ServerAliveIs((ICommunicationObject)ServerConnection.serviceFromServer))
            {
                ServerConnection.ConnectToService();
            }
        }
#endregion


        public static void counterLogout()
        {
            if (counterWindow!=null)
            {
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}