using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using Procesta.CVClient.Class;
using Procesta.CVClient.Class.Exceptions;
using Procesta.CVClient.Class.Methords;
using System.IO;
using Procesta.CVClient.Class.CvCPropertes;
using Procesta.CVClient.ServerSideService;
using System.ComponentModel;
using System.ServiceModel;
using System.Data.Services.Client;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections;
namespace Procesta.CVClient
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window, INotifyPropertyChanged
    {
        #region Private Variables
        public ObservableCollection<AllUserAndTeam> _newMembers;
        private string _Option;
        private bool _IsBusy;

        public string Option
        {
            get
            {
                return this._Option;
            }
            set
            {
                this._Option = value;
                OnPropertyChanged("Option");
            }
        }
        public static Counter counter;
        public ObservableCollection<AllUserAndTeam> newMembers
        {
            get
            {
                return this._newMembers;
            }
            set
            {
                this._newMembers = value;
                OnPropertyChanged("newMembers");
            }
        }

        public bool IsBusy
        {
            get
            {
                return this._IsBusy;
            }
            set
            {
                this._IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        private List<AllUserAndTeam> AddRemovemembers = new List<AllUserAndTeam>();
        #endregion

        public Settings()
        {
            newMembers = new ObservableCollection<AllUserAndTeam>();
            this.InitializeComponent();
            Mouse.OverrideCursor = null;
            this.DataContext = this;
        }

        #region Memu Bar

        public ICommand MenuBarCommand
        {
            get { return new ReplayCommand(new Action<object>(this.menuBarButton_Click)); }
        }

        private void menuBarButton_Click(object obj)
        {
            try
            {
                this.HideAllPanel();
                this.Option = "ByDate";
                switch (obj as string)
                {
                    case "ProfileEdit":
                        new Task(new Action(() =>
                        {
                            this.ServiceConnectionChecker();
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.PanelCustInfoEdit.DataContext = ServerConnection.serviceFromServer.AccountDetails(counter.customerDetail.Username);
                            }));
                        })).Start();
                        this.PanelCustInfoEdit.Visibility = Visibility.Visible;
                        break;
                    case "ChangePassword":
                        this.PasswordChangeClear();
                        this.settingPanelChangePassword.Visibility = Visibility.Visible;
                        break;
                    case "RechargeHistory":
                        this.RechHisGridView.ItemsSource = null;
                        this.panelRechHis.Visibility = Visibility.Visible;
                        break;
                    case "UserLoginHistory":
                        this.logHisGridView.ItemsSource = null;
                        this.PanelLoginHistory.Visibility = Visibility.Visible;
                        break;
                    case "AddTeam":
                        TeamInfo newTeamInfo = new TeamInfo();
                        this.PanelNewTeam.DataContext = newTeamInfo;
                        new Task(new Action(() =>
                        {
                            this.ServiceConnectionChecker();
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.teamUserList.ItemsSource = ServerConnection.serviceFromServer.GetAllUsers();
                            }));
                        })).Start();
                        this.teamMemberList.ItemsSource = new ObservableCollection<AllUserAndTeam>();
                        this.PanelNewTeam.Visibility = Visibility.Visible;
                        break;
                    case "EditTeam":
                        new Task(new Action(() =>
                        {
                            this.ServiceConnectionChecker();
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.editTeamComboBoxTeamName.ItemsSource = ServerConnection.serviceFromServer.GetTeamName(counter.customerDetail.Username);
                                this.teamEditUserList.ItemsSource = ServerConnection.serviceFromServer.GetAllUsers();
                            }));
                        })).Start();
                        this.PanelEditTeam.Visibility = Visibility.Visible;
                        break;
                    case "TeamRechargeHistory":
                        this.RechHisGridView.ItemsSource = null;
                        this.TeamRechHisTeamName.ItemsSource = ServerConnection.serviceFromServer.GetTeamName(counter.customerDetail.Username);
                        this.PanelTeamRechHis.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Panel Profile Edit

        public ICommand ProfileImageBrowseCommand
        {
            get { return new ReplayCommand(new Action<object>(this.profileImagebrowseClick)); }
        }

        public ICommand ProfileUpdateCommand
        {
            get { return new ReplayCommand(new Action<object>(this.profileUpdateClick)); }
        }

        private void profileImagebrowseClick(object obj)
        {
            this.profileImageBrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Userinformation userInfo = obj as Userinformation;
                userInfo.UserImage = imageOpenDialogBox();
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.profileImageBrowse.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }

        private void profileUpdateClick(object obj)
        {
            this.profileUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Userinformation userInfo = obj as Userinformation;
                this.ServiceConnectionChecker();
                if (ServerConnection.serviceFromServer.AccountUpdate(counter.customerDetail.Username, userInfo))
                {
                    counter.customerDetail.Photo = userInfo.UserImage;
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CVsVariables.ERROR_MESSAGES[0, 6], CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CVsVariables.ERROR_MESSAGES[0, 7], CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.profileUpdate.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region Panel User Login History


        public ICommand LoginHistorySearchCommand
        {
            get { return new ReplayCommand(new Action<object>(this.loginHistorySearch_Click)); }
        }

        private void loginHistorySearch_Click(object obj)
        {
            this.logHisButtonSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            this.IsBusy = true;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    this.ServiceConnectionChecker();
                    switch (this.Option)
                    {
                        case "ByDate":
                            this.logHisGridView.ItemsSource = ServerConnection.serviceFromServer.UserLoginHistoryDate(counter.customerDetail.Username, (DateTime)dataList[0]);
                            break;
                        case "BetweenTwoDate":
                            this.logHisGridView.ItemsSource = ServerConnection.serviceFromServer.UserLoginHistoryTwoDate(counter.customerDetail.Username, (DateTime)dataList[1], (DateTime)dataList[2]);
                            break;
                        case "All":
                            this.logHisGridView.ItemsSource = ServerConnection.serviceFromServer.UserLoginHistoryAll(counter.customerDetail.Username);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.logHisButtonSearch.IsEnabled = true;
                Mouse.OverrideCursor = null;
                this.IsBusy = false;
            }

        }
        #endregion

        #region Panel User Recharge History

        public ICommand UserRechHisCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.userRechHis_Click)); }
        }

        private void userRechHis_Click(object obj)
        {
            this.RechHisButtonSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            this.IsBusy = true;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    this.ServiceConnectionChecker();
                    switch (this.Option)
                    {
                        case "ByDate":
                            this.RechHisGridView.ItemsSource = ServerConnection.serviceFromServer.UserRechargeHistoryDate(counter.customerDetail.Username, (DateTime)dataList[0]);
                            break;
                        case "BetweenTwoDate":
                            this.RechHisGridView.ItemsSource = ServerConnection.serviceFromServer.UserRechargeHistoryTwoDate(counter.customerDetail.Username, (DateTime)dataList[1], (DateTime)dataList[2]);
                            break;
                        case "All":
                            this.RechHisGridView.ItemsSource = ServerConnection.serviceFromServer.UserRechargeHistoryAll(counter.customerDetail.Username);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.RechHisButtonSearch.IsEnabled = true;
                Mouse.OverrideCursor = null;
                this.IsBusy = false;
            }
        }
        #endregion

        #region Panel New Team
        public ICommand TeamImageBrowseCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamImageBrowse_Click)); }
        }

        public ICommand TeamMemberAddCommand
        {
            get { return new ReplayCommand(new Action<object>(this.memberAdd_Click)); }
        }
        public ICommand TeamMemberRemoveCommand
        {
            get { return new ReplayCommand(new Action<object>(this.memberRemove_Click)); }
        }
        public ICommand TeamUpdateCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamUpdate_Click)); }
        }
        private void teamImageBrowse_Click(object obj)
        {
            this.teamImagebrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is TeamInfo)
                {
                    TeamInfo newTeamInfo = obj as TeamInfo;
                    newTeamInfo.TeamImage = this.imageOpenDialogBox();
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.teamImagebrowse.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }

        private void memberAdd_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is AllUserAndTeam)
                {
                    AllUserAndTeam newTeamMember = obj as AllUserAndTeam;
                    var teamMembers = this.teamMemberList.ItemsSource as ObservableCollection<AllUserAndTeam>;
                    if (teamMembers.FirstOrDefault(x => x.Name.Equals(newTeamMember.Name)) == null)
                    {
                        teamMembers.Add(newTeamMember);
                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show("This user is already a member.", CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void memberRemove_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is AllUserAndTeam)
                {
                    (this.teamMemberList.ItemsSource as ObservableCollection<AllUserAndTeam>).Remove(obj as AllUserAndTeam);
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">
        /// obj[0] =  TeamInfo
        /// obj[1] = AllUserAndTeam
        /// </param>
        private void teamUpdate_Click(object obj)
        {
            this.teamUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    this.ServiceConnectionChecker();
                    ArrayList dataList = obj as ArrayList;
                    TeamInfo newTeamInfo= dataList[0] as TeamInfo;
                    newTeamInfo.TeamAdmin=counter.customerDetail.Username;
                    if (ServerConnection.serviceFromServer.AddNewTeam(newTeamInfo, new List<AllUserAndTeam>(dataList[1] as ObservableCollection<AllUserAndTeam>)))
                    {
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show(CVsVariables.ERROR_MESSAGES[0, 10], CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.teamUpdate.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region Panel Edit Team

        public ICommand GetTeamInfoCommand
        {
            get { return new ReplayCommand(new Action<object>(this.getTeamInfo_SelectionChange)); }
        }

        public ICommand TeamEditAddCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamEditMemberAdd_Click)); }
        }

        public ICommand TeamEditRemoveCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamEditMemberDelete_Click)); }
        }

        public ICommand TeamEditImageBrowseCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamEditImageBrowse_Click)); }
        }

        public ICommand TeamEditUpdateCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamEditUpdate_Click)); }
        }

        private void getTeamInfo_SelectionChange(object obj)
        {
            this.IsBusy = true;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string teamName = obj as String;
                this.ServiceConnectionChecker();
                teamEditMemberList.ItemsSource =new ObservableCollection<AllUserAndTeam>(ServerConnection.serviceFromServer.GetTeamMember(teamName));
                TeamInfo teamInformation = new TeamInfo();
                teamInformation.TeamImage = ServerConnection.serviceFromServer.GetTeamLogo(teamName);
                this.editTeamImageBorder.DataContext = teamInformation;
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.IsBusy = false;
                Mouse.OverrideCursor = null;
            }
        }

        private void teamEditMemberAdd_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is AllUserAndTeam)
                {
                    AllUserAndTeam newTeamMember = obj as AllUserAndTeam;
                    newTeamMember.AddOrDelete = true;
                    var teamMembers = this.teamEditMemberList.ItemsSource as ObservableCollection<AllUserAndTeam>;
                    if (teamMembers.FirstOrDefault(x => x.Name.Equals(newTeamMember.Name)) == null)
                    {
                        teamMembers.Add(newTeamMember);
                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show("This user is already a member.", CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void teamEditMemberDelete_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is AllUserAndTeam)
                {
                    var teamMember = obj as AllUserAndTeam;
                    teamMember.AddOrDelete = false;
                    (this.teamEditMemberList.ItemsSource as ObservableCollection<AllUserAndTeam>).Remove(teamMember);
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        private void teamEditImageBrowse_Click(object obj)
        {
            this.editTeamBrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is TeamInfo)
                {
                    TeamInfo newTeamInfo = obj as TeamInfo;
                    newTeamInfo.TeamImage = this.imageOpenDialogBox();
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.editTeamBrowse.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }
        private void teamEditUpdate_Click(object obj)
        {
            this.teamEditUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    this.ServiceConnectionChecker();
                    if (ServerConnection.serviceFromServer.UpdateTeam(dataList[0] as String, new List<AllUserAndTeam>(dataList[1] as ObservableCollection<AllUserAndTeam>)))
                    {
                        this.AddRemovemembers.Clear();
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show(CVsVariables.ERROR_MESSAGES[0, 6], CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.teamEditUpdate.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region Panel Change Password

        public ICommand ChangePasswordCommand
        {
            get { return new ReplayCommand(new Action<object>(this.changePassword_Click)); }
        }

        private void changePassword_Click(object obj)
        {
            this.passwordChangeUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.ServiceConnectionChecker();
                if (ServerConnection.serviceFromServer.PasswordChange(counter.customerDetail.Username, obj as String))
                {
                    MiraculasProperty.password = obj as String;
                    DXMessageBox.Show(CVsVariables.ERROR_MESSAGES[0, 6], CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.passwordChangeUpdate.IsEnabled = true;
            }
        }
        #endregion

        #region panel Team Rechage History

        public ICommand TeamRechHisCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.TeamRechHis_Click)); }
        }

        private void TeamRechHis_Click(object obj)
        {
            this.TeamRechHistorySearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            this.IsBusy = true;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    this.ServiceConnectionChecker();
                    switch (this.Option)
                    {
                        case "ByDate":
                            this.TeamRechHisGridView.ItemsSource = ServerConnection.serviceFromServer.TeamRechargeHistoryDate(dataList[3] as String, (DateTime)dataList[0]);
                            break;
                        case "BetweenTwoDate":
                            this.TeamRechHisGridView.ItemsSource = ServerConnection.serviceFromServer.TeamRechargeHistoryTwoDate(dataList[3] as String, (DateTime)dataList[1], (DateTime)dataList[2]);
                            break;
                        case "All":
                            this.TeamRechHisGridView.ItemsSource = ServerConnection.serviceFromServer.TeamRechargeHistoryAll(dataList[3] as String);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.TeamRechHistorySearch.IsEnabled = true;
                Mouse.OverrideCursor = null;
                this.IsBusy = false;
            }
        }
        #endregion

        #region Private Metods


        private byte[] imageOpenDialogBox()
        {
            System.Windows.Forms.OpenFileDialog fileOpenDialogBox = new System.Windows.Forms.OpenFileDialog();
            fileOpenDialogBox.Filter = "JPEG|*.jpg|BMP|*.bmp|PNG|*.png";
            fileOpenDialogBox.Title = "Select a Images";
            fileOpenDialogBox.FilterIndex = 1;
            fileOpenDialogBox.RestoreDirectory = true;
            byte[] imageInbytes = null;
            if (fileOpenDialogBox.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                FileInfo ImageInfo = new FileInfo(fileOpenDialogBox.FileName);
                if (ImageInfo.Length < 819200)
                {
                    imageInbytes = new MiraculousMethods().imageToByteArray(fileOpenDialogBox.FileName);
                }
                else
                {
                    DXMessageBox.Show(CVsVariables.ERROR_MESSAGES[0, 5], CVsVariables.SOTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            return imageInbytes;
        }

        private void HideAllPanel()
        {
            var visiableGrids = this.LayoutRoot.Children.OfType<Grid>().Where(x => x.Visibility == Visibility.Visible);
            foreach (Grid visiableGrid in visiableGrids)
            {
                visiableGrid.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// Clear New Team Items
        /// </summary>
        private void NewTeamitemClear()
        {
            //this.newTeamImage.Source = null;
            //this.newTeamTextBoxName.Text = string.Empty;
            //this.newTeamComboBoxMemberName.Text = string.Empty;
            //this.newMembers.Clear();
        }
        /// <summary>
        /// Check Service connection null Or Not
        /// </summary>
        private void ServiceConnectionChecker()
        {
            if (ServerConnection.serviceFromServer == null || !ServerConnection.ServerAliveIs((ICommunicationObject)ServerConnection.serviceFromServer))
            {
                ServerConnection.ConnectToService();
            }
        }

        private void RecharegHandelClear()
        {
            //this.RechHisButtonSearch.Click -= new RoutedEventHandler(RechHisButtonSearchClick);
            //this.RechHisButtonSearch.Click -= new RoutedEventHandler(RechHisButtonSearchForTeamClick);
        }

        private void PasswordChangeClear()
        {
            this.settingChangeNewPassword.Password = string.Empty;
            this.settingChangeOldPassword.Password = string.Empty;
            this.settingChnageConPassword.Password = string.Empty;
        }
        #endregion

        #region Propery Change
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion
    }
}