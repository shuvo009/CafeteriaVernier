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
using DevExpress.Xpf.Core;
using Procesta.CVClient.Class;
using Procesta.CVClient.Class.CvCPropertes;
using Procesta.CVClient.Class.Exceptions;
using Procesta.CVClient.Class.Methords;
using System.Diagnostics;
using Procesta.CVClient.ServerSideService;
using System.ServiceModel;
using System.ComponentModel;
namespace Procesta.CVClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return this._IsBusy; }
            set 
            {
                this._IsBusy = value;
                onPropertyChnage("IsBusy");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.LogtextboxUserName.Focus();
            this.PanelLogin.DataContext = new UserInfoViewer();
        }

        private void testCloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region Panel Login
        public ICommand CustomerLoginCommand
        {
            get { return new ReplayCommand(new Action<object>(this.customerLogin)); }
        }

        private void customerLogin(object obj) 
        {
            this.IsBusy = true;
            this.CustomerLogin.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                UserInfoViewer loggedInCutomer = obj as UserInfoViewer;
                if (String.IsNullOrEmpty(loggedInCutomer.Username) || String.IsNullOrWhiteSpace(loggedInCutomer.Username) || String.IsNullOrEmpty(loggedInCutomer.Password) || string.IsNullOrWhiteSpace(loggedInCutomer.Password))
                {
                    DXMessageBox.Show("Please enter Username and Password", CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ServerConnection.serviceFromServer == null || !ServerConnection.ServerAliveIs((ICommunicationObject)ServerConnection.serviceFromServer))
                {
                    ServerConnection.ConnectToService();
                }
                if (String.IsNullOrEmpty(loggedInCutomer.TeamName) || String.IsNullOrWhiteSpace(loggedInCutomer.TeamName))
                {
                    List<Int64> userInformation = ServerConnection.serviceFromServer.UserLogin(loggedInCutomer.Username, loggedInCutomer.Password, Properties.Settings.Default.CounterNumber);
                    loggedInCutomer.Minutes = userInformation.First();
                    loggedInCutomer.LoginHistoryID = userInformation.Last();
                }
                else
                {
                    List<Int64> teamInformation = ServerConnection.serviceFromServer.TeamLogin(loggedInCutomer.Username, loggedInCutomer.Password, Properties.Settings.Default.CounterNumber, loggedInCutomer.TeamName);
                    loggedInCutomer.Minutes = teamInformation.First();
                    loggedInCutomer.LoginHistoryID = teamInformation.Last();
                }
                loggedInCutomer.Photo = ServerConnection.serviceFromServer.GetUserImage(loggedInCutomer.Username);
                Counter.CounterWindow(loggedInCutomer);
                this.Close();
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.CustomerLogin.IsEnabled = true;
                Mouse.OverrideCursor = null;
                this.IsBusy = false;
            }
        }
        #endregion

        #region Panel shudown
        /// <summary>
        /// Restart Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogImageButtonRestartClcik(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Process.Start("shutdown","/r /t 0"); 
            }
            catch (Exception error)
            {
                DXMessageBox.Show(error.Message, CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Shutdown Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogImageButtonShutdownClick(object sender, System.Windows.RoutedEventArgs e)
        {
        	try
            {
                Process.Start("shutdown","/s /t 0"); 
            }
            catch (Exception error)
            {
                DXMessageBox.Show(error.Message, CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void onPropertyChnage(string propertyName)
        {
            if (this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
