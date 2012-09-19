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
using System.IO;
using Procesta.CvServer.Class;
using Procesta.CvServer.Class.CounterInfo;
using Procesta.CvServer.Class.Methods;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using System.Resources;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Reflection;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Procesta.CvServer.UserControls;
using System.Threading.Tasks;
using Procesta.CvServer.EntityFramework;
using Telerik.Windows.Controls;
using Procesta.CvServer.Reports;
using DevExpress.Xpf.Printing;
using System.Collections;
using InternetAccessories;
using Procesta.CvServer.ClientNotification;
using Telerik.Windows;
using System.Data.Objects;
namespace Procesta.CvServer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>`
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Private Variables
        private BackgroundWorker sendMailWorker = new BackgroundWorker();
        private BackgroundWorker dbBackupWorker = new BackgroundWorker();
        private BackgroundWorker dbRestoreWorker = new BackgroundWorker();
        private ModelEmployee _LoginEmployee;
        private ObservableCollection<ModelBillConfig> _BillConfigInfo;
        private string _Option = "ByDate";
        private string _SubOption = "Equal";
        #endregion

        #region Propertys

        public ModelEmployee LoginEmployee
        {
            get { return this._LoginEmployee; }
            set 
            {
                this._LoginEmployee = value;
                this.OnPropertyChanged("LoginEmployee");
            }
        }
        public ObservableCollection<ModelBillConfig> BillConfigInfo
        {
            get { return this._BillConfigInfo; }
            set
            {
                if (this._BillConfigInfo != value)
                {
                    this._BillConfigInfo = value;
                    this.OnPropertyChanged("BillConfigInfo");
                }
            }
        }
        public string Option
        {
            get { return this._Option; }
            set
            {
                if (this._Option != value)
                {
                    this._Option = value;
                    this.OnPropertyChanged("Option");
                }
            }
        }
        public string SubOption
        {
            get { return this._SubOption; }
            set
            {
                if (this._SubOption != value)
                {
                    this._SubOption = value;
                    this.OnPropertyChanged("SubOption");
                }
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
         
            sendMailWorker.WorkerReportsProgress = true;
            sendMailWorker.WorkerSupportsCancellation = true;
            sendMailWorker.DoWork += new DoWorkEventHandler(sendMailWorker_DoWork);
            sendMailWorker.ProgressChanged += new ProgressChangedEventHandler(sendMailWorker_ProgressChanged);
            sendMailWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(sendMailWorker_RunWorkerCompleted);

            dbBackupWorker.WorkerReportsProgress = true;
            dbBackupWorker.DoWork += new DoWorkEventHandler(dbBackupWorker_DoWork);
            dbBackupWorker.ProgressChanged += new ProgressChangedEventHandler(dbBackupWorker_ProgressChanged);
            dbBackupWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dbBackupWorker_RunWorkerCompleted);

            dbRestoreWorker.WorkerReportsProgress = true;
            dbRestoreWorker.DoWork += new DoWorkEventHandler(dbRestoreWorker_DoWork);
            dbRestoreWorker.ProgressChanged += new ProgressChangedEventHandler(dbRestoreWorker_ProgressChanged);
            dbRestoreWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dbRestoreWorker_RunWorkerCompleted);

            
        }

        private void Window_Load(object sender, System.Windows.RoutedEventArgs e)
        {
            this.LoginBusyIndicator.Visibility = Visibility.Visible;
        }

        #region Counter Information
        public ICommand ClientShoutdownCommand
        {
            get { return new ReplayCommand(new Action<object>(this.clientShoutdown_Click)); }
        }

        public ICommand ClientRestartCommand
        {
            get { return new ReplayCommand(new Action<object>(this.clientRestart_Click)); }
        }

        public ICommand ClientLogoffCommand
        {
            get { return new ReplayCommand(new Action<object>(this.clientLogoff_Click)); }
        }


        private void clientShoutdown_Click(object obj)
        {
            ClientNotificationClient clientNotify = new ClientNotificationClient("NetNamedPipeBinding_IClientNotification");
            clientNotify.setCommand(new CommandData { Command = Commands.Shutdown, CounterNumber = obj.ToString() });
            clientNotify.Close();
        }

        private void clientRestart_Click(object obj)
        {
            ClientNotificationClient clientNotify = new ClientNotificationClient("NetNamedPipeBinding_IClientNotification");
            clientNotify.setCommand(new CommandData { Command = Commands.Restart, CounterNumber = obj.ToString() });
            clientNotify.Close();
        }

        private void clientLogoff_Click(object obj)
        {
            ClientNotificationClient clientNotify = new ClientNotificationClient("NetNamedPipeBinding_IClientNotification");
            clientNotify.setCommand(new CommandData { Command = Commands.AccountLogout, CounterNumber = obj.ToString() });
            clientNotify.Close();
        }

        private void tileView1_TileStateChanged(object sender, RadRoutedEventArgs e)
        {
            RadTileViewItem item = e.OriginalSource as RadTileViewItem;
            if (item != null)
            {
                RadFluidContentControl fluid = item.ChildrenOfType<RadFluidContentControl>().FirstOrDefault();
                if (fluid != null)
                {
                    switch (item.TileState)
                    {
                        case TileViewItemState.Maximized:
                            fluid.State = FluidContentControlState.Large;
                            break;
                        case TileViewItemState.Minimized:
                            fluid.State = FluidContentControlState.Normal;
                            break;
                        case TileViewItemState.Restored:
                            fluid.State = FluidContentControlState.Normal;
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        #endregion

        #region Menu Button
        public ICommand MainMenuCommand
        {
            get { return new ReplayCommand(new Action<object>(this.mainMenuClick)); }
        }

        private void mainMenuClick(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (this.LoginEmployee==null || this.LoginEmployee.Permissions.AsParallel().FirstOrDefault(x => x.Item.Equals(obj.ToString())) == null)
                {
                    return;
                }
                this.hidePanels();
                switch (obj.ToString())
                {
                    case "NewEmployee":
                        new Task(
                            new Action(() =>
                                {
                                    using (Cafeteria_Vernier_dbEntities CVDatabase = new Cafeteria_Vernier_dbEntities())
                                    {
                                        ViewEmployPermissions defultParmissions = new ViewEmployPermissions();
                                        ObservableCollection<ModelEmployee> employeeinfoList = new ObservableCollection<ModelEmployee>(from employeeInfo in CVDatabase.Employees.ToList().Where(x => x.EmployeeID.Trim() != "Admin")
                                                                                                                    select new ModelEmployee
                                                                                                                    {
                                                                                                                        UserImage = employeeInfo.UserImage,
                                                                                                                        Address = employeeInfo.Address,
                                                                                                                        Name = employeeInfo.EmployeeID,
                                                                                                                        Password = employeeInfo.Password,
                                                                                                                        FirstPassword = employeeInfo.Password,
                                                                                                                        PhoneNmber = employeeInfo.Phone,
                                                                                                                        Permissions = new ObservableCollection<ModelEmployPermissions>
                                                                                                                        (
                                                                                                                            (from permissionDb in defultParmissions select new ModelEmployPermissions { SupperTip = permissionDb.SupperTip, Priority = permissionDb.Priority, Setting = permissionDb.Setting, Permission = employeeInfo.EmployeePermissions.FirstOrDefault(x => x.Privilege.Trim().Equals(permissionDb.Item, StringComparison.InvariantCultureIgnoreCase) && permissionDb.Setting == x.SettingPrivilage) != null ? true : false }).OrderBy(x => x.Priority)
                                                                                                                        )
                                                                                                                    });
                                        this.Dispatcher.BeginInvoke(
                                            new Action(() => 
                                            {
                                                this.EmployeeGridView.ItemsSource = employeeinfoList;
                                                this.selectGridViewFirstItem(this.EmployeeGridView);
                                            }),DispatcherPriority.DataBind);
                                                
                                    }
                                })).Start();

                        this.PanelNewEmploy.Visibility = Visibility.Visible;
                        break;
                    case "CountersInformation":
                        this.PanelCounterView.Visibility = Visibility.Visible;
                        break;
                    case "NewCustomer":
                        new Task(
                            new Action(() =>
                                {
                                    using (Cafeteria_Vernier_dbEntities CVDatabase = new Cafeteria_Vernier_dbEntities())
                                    {
                                        ObservableCollection<ModelCustomer> customerCollection = new ObservableCollection<ModelCustomer>(from customerInfo in CVDatabase.CustomerInformations
                                                                                                                                            select new ModelCustomer
                                                                                                                                            {
                                                                                                                                                UserName = customerInfo.UserID,
                                                                                                                                                Address = customerInfo.Address,
                                                                                                                                                Email = customerInfo.Email,
                                                                                                                                                Image = customerInfo.Logo,
                                                                                                                                                JoinDate = customerInfo.JoinDate,
                                                                                                                                                NationalID = customerInfo.NationalID,
                                                                                                                                                Name = customerInfo.Name,
                                                                                                                                                Phone = customerInfo.Phone,
                                                                                                                                                Minutes = customerInfo.CustomerAccount.Minutes,
                                                                                                                                                CheckPassword = customerInfo.CustomerAccount.Password,
                                                                                                                                                Password = customerInfo.CustomerAccount.Password
                                                                                                                                            });
                                        this.Dispatcher.BeginInvoke(
                                            new Action(() =>
                                                {
                                                    ICollectionView ProductInfoView = CollectionViewSource.GetDefaultView(customerCollection);
                                                    this.CustomerList.ItemsSource = customerCollection;
                                                    this.selectListBoxFirstItem(this.CustomerList);
                                                    new CustomerInfoSearch(ProductInfoView, this.CustomerTxtSearch);
                                                }),DispatcherPriority.DataBind);
                                    }
                                })).Start();
                        this.PanelNewCustomer.Visibility = Visibility.Visible;
                        break;
                    case "NewTeam":
                            new Task(
                            new Action(() => 
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                                {
                                    var teamsQuery = new ObservableCollection<ModelTeamInfo>(from teamInfo in cvDatabase.Teams.ToList()
                                                                                                select new ModelTeamInfo
                                                                                                {
                                                                                                    AdminName = teamInfo.AdminName,
                                                                                                    Image = teamInfo.Logo,
                                                                                                    JoinDate = teamInfo.JoinDate,
                                                                                                    Minutes = teamInfo.TeamAccount.Minutes,
                                                                                                    Name = teamInfo.Name,
                                                                                                    TeamMemberList = new ObservableCollection<ModelCommonUse>(from member in teamInfo.TeamMembers
                                                                                                                                                            where member.Name!=null
                                                                                                                                                            select new ModelCommonUse 
                                                                                                                                                            {
                                                                                                                                                                UserName=member.UserID,
                                                                                                                                                                Image=member.CustomerInformation.Logo
                                                                                                                                                            })
                                                                                                });
                                    this.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        this.TeamGridView.ItemsSource = teamsQuery;
                                        this.selectGridViewFirstItem(this.TeamGridView);
                                    }), DispatcherPriority.DataBind);
                                }
                            })).Start();
                                
                            new Task(
                                new Action(() => 
                                {
                                    ObservableCollection<ModelCommonUse> customerShotInfo = this.customerInfo();
                                        this.Dispatcher.BeginInvoke(
                                            new Action(() => 
                                            {
                                                ICollectionView userInfoView = CollectionViewSource.GetDefaultView(customerShotInfo);
                                                new CommonInfoSearch(userInfoView, this.TeamUserSearch);
                                                this.TeamExistUserList.ItemsSource = customerShotInfo;
                                                this.TeamAdminName.ItemsSource = customerShotInfo;
                                            }),DispatcherPriority.DataBind);
                                })).Start();
                            this.PanelNewTeam.Visibility = Visibility.Visible;
                        break;
                    case "AccountRecharge":
                        new Task(
                            new Action(() =>
                                {
                                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                                    {
                                        ObservableCollection<ModelCustomer> userQuery = new ObservableCollection<ModelCustomer>(from userInfo in cvDatabase.CustomerInformations
                                                                                                                                    select new ModelCustomer
                                                                                                                                    {
                                                                                                                                        Image = userInfo.Logo,
                                                                                                                                        Name = userInfo.UserID,
                                                                                                                                        Minutes=userInfo.CustomerAccount.Minutes
                                                                                                                                    });
                                        this.Dispatcher.BeginInvoke(
                                            new Action(() =>
                                            {
                                                ICollectionView userInfoView = CollectionViewSource.GetDefaultView(userQuery);
                                                new AccountRechargeSearchs(userInfoView, this.AccountRecUserSearch);
                                                this.AccountRecUser.ItemsSource = userQuery;
                                                this.selectListBoxFirstItem(this.AccountRecUser);
                                            }));
                                    }
                                })).Start();
                        new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<ModelTeamInfo> teamQuery = new ObservableCollection<ModelTeamInfo>(from teamInfo in cvDatabase.Teams
                                                                                                                                select new ModelTeamInfo
                                                                                                                                {
                                                                                                                                    Image = teamInfo.Logo,
                                                                                                                                    Name = teamInfo.Name,
                                                                                                                                    Minutes=teamInfo.TeamAccount.Minutes
                                                                                                                                });
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            ICollectionView teamInfoView = CollectionViewSource.GetDefaultView(teamQuery);
                                            new AccountRechargeSearchs(teamInfoView, this.AccountRecTeamSearch);
                                            this.AccountRecTeam.ItemsSource = teamQuery;
                                            this.selectListBoxFirstItem(this.AccountRecTeam);
                                        }));
                                }
                            })).Start();
                        this.PanelAccountRecharge.Visibility = Visibility.Visible;
                        break;
                    case "Cash":
                        this.PanelCahView.Visibility = Visibility.Visible;
                        break;
                    case "CashHistory":
                        this.PanelCashHistory.Visibility = Visibility.Visible;
                        break;
                    case "Summary":
                        this.PanelSummary.Visibility = Visibility.Visible;
                        break;
                    case "RechargeHistory":
                        new Task(
                            new Action(() =>
                                {
                                    using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                    {
                                        ObservableCollection<ModelCommonUse> cutomerShortInfo = this.customerInfo();
                                        this.Dispatcher.BeginInvoke(
                                            new Action(() =>
                                            {
                                                this.ResHisCustomerComboBox.ItemsSource = cutomerShortInfo;
                                            }));
                                    }
                                })).Start();
                        new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<ModelCommonUse> teamShortInfo = new ObservableCollection<ModelCommonUse>(cvDatabse.Teams.Select(x =>
                                                                                                                                                    new ModelCommonUse
                                                                                                                                                    {
                                                                                                                                                        UserName = x.Name,
                                                                                                                                                        Image = x.Logo
                                                                                                                                                    }));
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            this.ResHisTeamComboBox.ItemsSource = teamShortInfo;
                                        }));
                                }
                            })).Start();
                        this.PanelRechargeHistoryView.Visibility = Visibility.Visible;
                        break;
                    case "CustomerLoginHistory":
                        new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<ModelCommonUse> customerShortInfo = this.customerInfo();
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            this.LogHisComboBox.ItemsSource = customerShortInfo;
                                        }));
                                }
                            })).Start();
                        this.PanelLoginHistory.Visibility = Visibility.Visible;
                        break;
                    case "CustomerMaintenance":
                            new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<ModelCommonUse> cusromerShortInfo = this.customerInfo();
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            this.UserMaintenanceCutomer.ItemsSource = cusromerShortInfo;
                                        }));
                                }
                            })).Start();
                            this.Option = "ByName";
                            this.SubOption = "ByDate";
                        this.PanelUserMaintenance.Visibility = Visibility.Visible;
                        break;
                    case "TeamMaintenance":
                            new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<ModelCommonUse> teamShortInfo = new ObservableCollection<ModelCommonUse>(cvDatabse.Teams.Select(x =>
                                                                                                                                                        new ModelCommonUse
                                                                                                                                                        {
                                                                                                                                                            UserName = x.Name,
                                                                                                                                                            Image = x.Logo
                                                                                                                                                        }));
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            this.temMainTeamName.ItemsSource = teamShortInfo;
                                        }));
                                }
                            })).Start();
                            this.Option = "ByName";
                            this.SubOption = "ByDate";
                        this.PanelTeamMainTenance.Visibility = Visibility.Visible;
                        break;
                    case "SendEmail":
                            new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<ModelCommonUse> customerShortInfo = this.customerInfo();
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            this.sendMailUsers.ItemsSource = customerShortInfo;
                                        }));
                                }
                            })).Start();
                            this.Option = "OneByOne";
                            this.PanelSendMail.Visibility = Visibility.Visible;
                        break;
                    case "Database":
                        this.PanelDatabaseBackupRestore.Visibility = Visibility.Visible;
                        break;
                    case "Setting":
                        this.PanelSetting.Visibility = Visibility.Visible;
                        break;
                    case "CustomerStatusReset":
                        new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<CustomerAccount> logginCustomers = new ObservableCollection<CustomerAccount>(cvDatabse.CustomerAccounts.Where(x => x.Status == true));
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            this.userResGrid.ItemsSource = logginCustomers;

                                        }));
                                }

                            })).Start();
                        new Task(
                            new Action(() =>
                            {
                                using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
                                {
                                    ObservableCollection<TeamAccount> logginCustomers = new ObservableCollection<TeamAccount>(cvDatabse.TeamAccounts.Where(x => x.Status == true));
                                    this.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                        {
                                            this.TeamResGrid.ItemsSource = logginCustomers;

                                        }));
                                }

                            })).Start();
                        this.PanelStatusReset.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog(string.Format("Menu >> {0} on Click", obj.ToString()), ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region counterInfoViewer
        private void counterInfoViewerStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Telerik.Windows.Controls.RadTileViewItem radItem = e.OriginalSource as Telerik.Windows.Controls.RadTileViewItem;
            if (radItem != null)
            {
                //CommonUse.CounterStatues tempCounterStates = radItem.DataContext as CommonUse.CounterStatues;
                //if (tempCounterStates != null)
                //{
                //    tempCounterStates.State = radItem.TileState;
                //}
            }
        }
        #endregion

        #region Panel Employee Login
        public ICommand EmployeeLoginCommand
        {
            get { return new ReplayCommand(new Action<object>(this.employeeLogin_Click)); }
        }
        /// <summary>
        /// Employee Login Operation is here.
        /// </summary>
        /// <param name="obj">Type of ModelEmployee</param>
        private void employeeLogin_Click(object obj)
        {
            this.EmployeeLogin.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    ModelEmployee employeeLoginInfo = obj as ModelEmployee;
                    if (!String.IsNullOrEmpty(employeeLoginInfo.Name) && !String.IsNullOrEmpty(employeeLoginInfo.Password))
                    {
                        this.LoginBusyIndicator.IsBusy = true;
                        Task LoginTask = new Task(new Action(() => 
                        {
                            using (Cafeteria_Vernier_dbEntities CVDatabase= new Cafeteria_Vernier_dbEntities())
                            {
                                ViewEmployPermissions defultParmissions = new ViewEmployPermissions();
                                LoginEmployee = null;
                                // Query in the database and get employee information and his all permissions.
                                LoginEmployee = (from employeeInfo in CVDatabase.Employees.ToList()
                                                 where employeeInfo.EmployeeID.Trim().Equals(employeeLoginInfo.Name, StringComparison.InvariantCultureIgnoreCase) && employeeInfo.Password.Trim().Equals(employeeLoginInfo.Password) 
                                                 select new ModelEmployee { UserImage=employeeInfo.UserImage, Address = employeeInfo.Address, Name = employeeInfo.EmployeeID, Password = employeeInfo.Password, PhoneNmber = employeeInfo.Phone,
                                                     Permissions = (new ObservableCollection<ModelEmployPermissions>(new ObservableCollection<ModelEmployPermissions>
                                                         (from employeeParmisionbd in employeeInfo.EmployeePermissions join employeeParmissions in defultParmissions 
                                                          on employeeParmisionbd.Privilege.Trim() equals employeeParmissions.Item 
                                                          orderby employeeParmissions.Priority
                                                          select new ModelEmployPermissions { Item = employeeParmissions.Item, Permission = employeeParmissions.Permission, Setting = employeeParmissions.Setting, ImagePath = employeeParmissions.ImagePath, KeboardShortcut = employeeParmissions.KeboardShortcut, Priority = employeeParmissions.Priority, ScreenTip = employeeParmissions.ScreenTip, SupperTip = employeeParmissions.SupperTip })
                                                          .Distinct(new ParmissionIequality()))) }).FirstOrDefault();
                                if (LoginEmployee != null)
                                {
                                    this.Dispatcher.Invoke(new Action(() =>
                                    {
                                        //Build the main menu
                                        this.MainMenu.ItemsSource = LoginEmployee.Permissions.Where(x => x.Priority != 0 && x.Setting == null).OrderBy(x => x.Priority);
                                        var yy = LoginEmployee.Permissions.Where(x => x.Item.Equals("Setting") && x.Setting != null).OrderBy(x => x.Priority);
                                        this.SettingMenu.ItemsSource = yy;
                                        this.mainMenuClick("Home");
                                        this.MainMenu.Visibility = Visibility.Visible;
                                        this.LoginBusyIndicator.IsBusy = false;
                                        this.LoginBusyIndicator.Visibility = Visibility.Hidden;
                                        this.PanelLogerInfo.Visibility = Visibility.Visible;
                                    }));
                                }
                                else
                                {
                                    this.Dispatcher.Invoke(new Action(() =>
                                    {
                                        Mouse.OverrideCursor = null;
                                        DXMessageBox.Show("Username or Password is not correct.", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                        this.LoginBusyIndicator.IsBusy = false;
                                    }), DispatcherPriority.Normal);
                                }
                            }
                        }));
                        LoginTask.Start();
                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show("Username and Password are empty", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show("Username and Password are empty",CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Login Button Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeLogin.IsEnabled = true;
            }
        }
        #endregion

        #region Panel LogerInfo or logout
        public ICommand LogoutCommand
        {
            get { return new ReplayCommand(new Action<object>(this.logout_Click)); }
        }
        /// <summary>
        /// Employee Logout
        /// </summary>
        /// <param name="obj"></param>
        private void logout_Click(object obj)
        {
            this.EmployeeLogout.IsEnabled = false;
            try
            {
                if (obj != null)
                {
                    if (DXMessageBox.Show("Logout ! are you sure ?", CvVariables.SOFTWARE_NAME, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        this.MainMenu.Visibility = Visibility.Hidden;
                        this.hidePanels();
                        this.LoginBusyIndicator.Visibility = Visibility.Visible;
                        this.PanelLogerInfo.Visibility = Visibility.Hidden;
                        this.EmployeeLoginUsername.Focus();
                        ModelEmployee loginInfo = obj as ModelEmployee;
                        loginInfo.Name = loginInfo.Password = string.Empty;
                        this.LoginEmployee = null;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,0], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                }
              
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Employee Logout Click", ErrorException);
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeLogout.IsEnabled = true;
            }
        }
        #endregion

        #region Panel New Customer

        public ICommand NewCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.newCustomer_Click)); }
        }

        public ICommand UpdateCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.updateCustomer_Click)); }
        }

        public ICommand DeleteCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.deleteCustomer_Click)); }
        }
        
        public ICommand BrowseCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.browseCustomer_Click)); }
        }

        public ICommand WebCamCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.webCamCustomer_Click)); }
        }
        /// <summary>
        /// Add new Customer
        /// </summary>
        /// <param name="obj"> No parameter need</param>
        private void newCustomer_Click(object obj)
        {
            this.CustomerNew.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ModelCustomer newCustomer = new ModelCustomer();
                newCustomer.JoinDate = DateTime.Today;
                (this.CustomerList.ItemsSource as ObservableCollection<ModelCustomer>).Add(newCustomer);
                this.CustomerList.SelectedIndex = this.CustomerList.Items.IndexOf(newCustomer); 
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("New Customer Click", ErrorException);
                DXMessageBox.Show(ErrorException.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CustomerNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// Update or insert new Customer 
        /// </summary>
        /// <param name="obj">ListBox Selected Item</param>
        private void updateCustomer_Click(object obj)
        {
            this.CustomerUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj != null)
                {
                    this.CustomerUserName.GetBindingExpression(TextEdit.TextProperty);
                    this.CustomerPassword.GetBindingExpression(PasswordBoxEdit.PasswordProperty);
                    this.CustomerName.GetBindingExpression(TextEdit.TextProperty);
                    this.CustomerPhone.GetBindingExpression(TextEdit.TextProperty);
                    this.CustomerEmail.GetBindingExpression(TextEdit.TextProperty);
                    this.getValidationError(this.CustomerUserName, this.CustomerPassword, this.CustomerPhone, this.CustomerPhone, this.CustomerName, this.CustomerEmail);
                    using (Cafeteria_Vernier_dbEntities CVDatabase= new Cafeteria_Vernier_dbEntities())
                    {
                        ModelCustomer selectedCustomer = obj as ModelCustomer;
                        var customerExits = CVDatabase.CustomerInformations.FirstOrDefault(x=>x.UserID.Equals(selectedCustomer.UserName));
                        if (customerExits!=null)
                        {
                            if (this.LoginEmployee.Permissions.AsParallel().FirstOrDefault(x=>x.Item == ("EditCustomer"))!=null)
                            {
                                 Mouse.OverrideCursor = null;
                                 if (DXMessageBox.Show(CvVariables.ERROR_MESSAGES[1, 8], CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                 {
                                     Mouse.OverrideCursor = Cursors.Wait;
                                     customerExits.Address = selectedCustomer.Address;
                                     customerExits.CustomerAccount.Password = selectedCustomer.Password;
                                     customerExits.CustomerAccount.Minutes = selectedCustomer.Minutes;
                                     customerExits.Email = selectedCustomer.Email;
                                     customerExits.JoinDate = selectedCustomer.JoinDate;
                                     customerExits.Logo = selectedCustomer.Image;
                                     customerExits.NationalID = selectedCustomer.NationalID;
                                     customerExits.Phone = selectedCustomer.Phone;
                                     CVDatabase.SaveChanges();
                                     Mouse.OverrideCursor = null;
                                     DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 2], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                                 }
                                 else
                                 {
                                     return;
                                 }
                            }
                            else
                            {
                                Mouse.OverrideCursor = null;
                                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 1], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                        }
                        else
                        {
                            CVDatabase.CustomerInformations.AddObject(
                                new CustomerInformation 
                                { 
                                    Address = selectedCustomer.Address,
                                    Email = selectedCustomer.Email,
                                    JoinDate = selectedCustomer.JoinDate,
                                    Logo = selectedCustomer.Image,
                                    Name = selectedCustomer.Name,
                                    NationalID = selectedCustomer.NationalID,
                                    Phone = selectedCustomer.Phone,
                                    UserID = selectedCustomer.UserName,
                                    CustomerAccount = new CustomerAccount
                                         {
                                             Counternumber = 0, 
                                             Minutes = selectedCustomer.Minutes, 
                                             Password = selectedCustomer.Password,
                                             Status = false
                                         } 
                                });
                            CVDatabase.SaveChanges();
                            Mouse.OverrideCursor = null;
                            DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 3], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 1], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Customer Update Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CustomerUpdate.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete a Customer 
        /// </summary>
        /// <param name="obj">ListBox Selected Item</param>
        private void deleteCustomer_Click(object obj)
        {
            this.CustomerDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj != null)
                {
                    using (Cafeteria_Vernier_dbEntities CVDatabase= new Cafeteria_Vernier_dbEntities())
                    {
                        ModelCustomer deleteCustomerInfo = obj as ModelCustomer;
                        CVDatabase.CustomerInformations.DeleteObject(CVDatabase.CustomerInformations.First(x => x.UserID.Equals(deleteCustomerInfo.UserName)));
                        CVDatabase.SaveChanges();
                        (this.CustomerList.ItemsSource as ObservableCollection<ModelCustomer>).Remove(deleteCustomerInfo);
                        this.selectListBoxFirstItem(this.CustomerList);
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 1], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Customer Delete Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CustomerDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Browse a Customer Image
        /// </summary>
        /// <param name="obj">ListBox Selected Item</param>
        private void browseCustomer_Click(object obj)
        {
            this.CustomerBrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj != null)
                {
                    (obj as ModelCustomer).Image = this.selectImageFromFile();
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Customer Browse Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CustomerBrowse.IsEnabled = true;
            }
        }
        /// <summary>
        /// Snapshot from WebCam 
        /// </summary>
        /// <param name="obj">ListBox Selected Item</param>
        private void webCamCustomer_Click(object obj)
        {
            this.CustomerWebCam.IsEnabled = false;
            try
            {
                if (obj != null)
                {
                    byte[] imageInBytes = null;
                    imageInBytes = WebCamWindow.CaptureImage();
                    Mouse.OverrideCursor = Cursors.Wait;
                    if (imageInBytes != null)
                    {
                        (obj as ModelCustomer).Image = imageInBytes;
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Customer WebCam Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CustomerWebCam.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Account Recharge
        public ICommand AccountRechargeCommand
        {
            get { return new ReplayCommand(new Action<object>(this.accountRechargeClick)); }
        }
        /// <summary>
        /// Update Customer or Team Account
        /// Insert into Customer and Team Account recharge history 
        /// </summary>
        /// <param name="obj">
        /// obj[0] = ModelBillConfig
        /// obj[1] = ModelCustomer
        /// obj[2] = ModelTeamInfo
        /// obj[3] = CustomerInfo.IsChecked
        /// </param>
        private void accountRechargeClick(object obj)
        {
            this.RechareUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList= obj as ArrayList;
                    ModelBillConfig billInfo = dataList[0] as ModelBillConfig;
                    ModelCustomer customerInfo = dataList[1] as ModelCustomer;
                    ModelTeamInfo teamInfo = dataList[2] as ModelTeamInfo;
                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        if ((bool)dataList[3])
                        {
                            var userAccountInfo = cvDatabase.CustomerAccounts.First(x => x.UserID.Equals(customerInfo.Name));
                            userAccountInfo.Minutes += Convert.ToInt32(billInfo.Minutes);
                            cvDatabase.AddToUserRechargeHistories(
                                                        new UserRechargeHistory
                                                        {
                                                            AutoInc = default(long),
                                                            bill = Convert.ToInt32(billInfo.Amount),
                                                            DateWithTime = DateTime.Today,
                                                            EmployeeID = this.LoginEmployee.Name,
                                                            Minutes = Convert.ToInt32(billInfo.Minutes),
                                                            UserID = customerInfo.Name
                                                        });
                            customerInfo.Minutes += Convert.ToInt32(billInfo.Minutes);
                        }
                        else
                        {
                            var teamAccountInfo = cvDatabase.TeamAccounts.First(x => x.Name.Equals(teamInfo.Name));
                            teamAccountInfo.Minutes += Convert.ToInt32(billInfo.Minutes);
                            cvDatabase.AddToTeamRechargeHistories(
                                                            new TeamRechargeHistory
                                                            {
                                                                AutoInc = default(long),
                                                                bill = Convert.ToInt32(billInfo.Amount),
                                                                DateWithTime = DateTime.Today,
                                                                EmployeeID = this.LoginEmployee.Name,
                                                                Minutes = Convert.ToInt32(billInfo.Minutes),
                                                                Name = teamInfo.Name
                                                            });
                            teamInfo.Minutes += Convert.ToInt32(billInfo.Minutes);
                        }
                        var toDayCash = cvDatabase.Cashes.First(x => x.EntryDate == DateTime.Today);
                        toDayCash.Amount += Convert.ToDecimal(billInfo.Amount);
                        cvDatabase.SaveChanges();
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,2],CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Account Recharge", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.RechareUpdate.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Recharges history view
        public ICommand SearchRechargeHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.searchRechargeHistoryClick)); }
        }

        public ICommand DeleteRechareHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.deleteRechageHistoryClick)); }
        }

        public ICommand DeleteAllRechageHistoryClick
        {
            get { return new ReplayCommand(new Action<object>(this.deleteAllRechageHistoryClick)); }
        }
        /// <summary>
        /// Search recharge History
        /// </summary>
        /// <param name="obj">
        /// obj[0] = isCustomer(bool)
        /// obj[1] = Is by Name (bool)
        /// obj[2] = UserName
        /// obj[3] = TeamName
        /// obj[4] = date One
        /// obj[5] = date two
        /// </param>
        private void searchRechargeHistoryClick(object obj)
        {
            this.ResHiSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList datalist = obj as ArrayList;
                    DateTime firstDate=(DateTime)datalist[4];
                    DateTime secondDate=(DateTime)datalist[5];
                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        if ((bool)datalist[0]) // Is Customer
                        {
                            IQueryable<UserRechargeHistory> queryUserRechargeHistory = null;
                            this.ResHisGridView.Columns.OfType<GridViewDataColumn>().First(x => x.Name == "GridDataColumnName").DataMemberBinding = new Binding("UserID");
                            if ((bool)datalist[1]) // Is by Name
                            {
                                string userName = datalist[2].ToString();
                                switch (this.Option)
                                {
                                    case "ByDate":
                                        queryUserRechargeHistory = cvDatabase.UserRechargeHistories.Where(x => x.DateWithTime == firstDate && x.UserID == userName);
                                        break;
                                    case "BetweenTwoDate":
                                        queryUserRechargeHistory = cvDatabase.UserRechargeHistories.Where(x => x.DateWithTime >= firstDate && x.DateWithTime <= secondDate && x.UserID == userName);
                                        break;
                                    case "All":
                                        queryUserRechargeHistory = cvDatabase.UserRechargeHistories.Where(x => x.UserID == userName);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (this.Option)
                                {
                                    case "ByDate":
                                        queryUserRechargeHistory = cvDatabase.UserRechargeHistories.Where(x => x.DateWithTime == firstDate);
                                        break;
                                    case "BetweenTwoDate":
                                        queryUserRechargeHistory = cvDatabase.UserRechargeHistories.Where(x => x.DateWithTime >= firstDate && x.DateWithTime <= secondDate);
                                        break;
                                    case "All":
                                        queryUserRechargeHistory = cvDatabase.UserRechargeHistories;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            this.ResHisGridView.ItemsSource =new ObservableCollection<UserRechargeHistory>(queryUserRechargeHistory);
                        }
                        else
                        {
                            IQueryable<TeamRechargeHistory> queryTeamRechareHistory = null;
                            this.ResHisGridView.Columns.OfType<GridViewDataColumn>().First(x => x.Name == "GridDataColumnName").DataMemberBinding = new Binding("Name");
                            if ((bool)datalist[1]) // Is By Name
                            {
                                string teamName = datalist[3].ToString();
                                switch (this.Option)
                                {
                                    case "ByDate":
                                        queryTeamRechareHistory = cvDatabase.TeamRechargeHistories.Where(x => x.DateWithTime == firstDate && x.Name == teamName);
                                        break;
                                    case "BetweenTwoDate":
                                        queryTeamRechareHistory = cvDatabase.TeamRechargeHistories.Where(x => x.DateWithTime >= firstDate && x.DateWithTime <= secondDate && x.Name == teamName);
                                        break;
                                    case "All":
                                        queryTeamRechareHistory = cvDatabase.TeamRechargeHistories.Where(x => x.Name == teamName);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (this.Option)
                                {
                                    case "ByDate":
                                        queryTeamRechareHistory = cvDatabase.TeamRechargeHistories.Where(x => x.DateWithTime == firstDate);
                                        break;
                                    case "BetweenTwoDate":
                                        queryTeamRechareHistory = cvDatabase.TeamRechargeHistories.Where(x => x.DateWithTime >= firstDate && x.DateWithTime <= secondDate);
                                        break;
                                    case "All":
                                        queryTeamRechareHistory = cvDatabase.TeamRechargeHistories;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            this.ResHisGridView.ItemsSource = new ObservableCollection<TeamRechargeHistory>(queryTeamRechareHistory);
                        }
                    }
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Recharge History Search", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.ResHiSearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Selected Items
        /// </summary>
        /// <param name="obj">
        /// obj[0]=DataGrid.SelectedItems
        /// obj[1]=Is Customer
        /// </param>
        private void deleteRechageHistoryClick(object obj)
        {
            this.ResHisDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        if ((bool)dataList[1])
                        {
                            List<UserRechargeHistory> historyList = new List<UserRechargeHistory>();
                            foreach (var singleListory in dataList[0] as IEnumerable)
                            {
                                historyList.Add(singleListory as UserRechargeHistory);
                            }
                            foreach (UserRechargeHistory loginHistory in historyList)
                            {
                                cvDatabase.UserRechargeHistories.DeleteObject(cvDatabase.UserRechargeHistories.First(x=>x.DateWithTime == loginHistory.DateWithTime && x.EmployeeID.Equals(loginHistory.EmployeeID) && x.UserID.Equals(loginHistory.UserID)));
                                (this.ResHisGridView.ItemsSource as ObservableCollection<UserRechargeHistory>).Remove(loginHistory);
                            }
                        }
                        else
                        {
                            List<TeamRechargeHistory> historyList = new List<TeamRechargeHistory>();
                            foreach (var singleListory in dataList[0] as IEnumerable)
                            {
                                historyList.Add(singleListory as TeamRechargeHistory);
                            }
                            foreach (TeamRechargeHistory loginHistory in historyList)
                            {
                                cvDatabase.TeamRechargeHistories.DeleteObject(cvDatabase.TeamRechargeHistories.First(x=>x.DateWithTime == loginHistory.DateWithTime && x.EmployeeID.Equals(loginHistory.EmployeeID) && x.Name.Equals(loginHistory.Name)));
                                (this.ResHisGridView.ItemsSource as ObservableCollection<TeamRechargeHistory>).Remove(loginHistory);
                            }
                        }
                        cvDatabase.SaveChanges();
                    }
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Recharge History Delete", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.ResHisDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete all History
        /// </summary>
        /// <param name="obj">
        /// obj[0] = DataGrid.itemSource
        /// objj[1] = Is Customer 
        /// </param>
        private void deleteAllRechageHistoryClick(object obj)
        {
            this.ResHisDeleteAll.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        if ((bool)dataList[1])
                        {
                            ObservableCollection<UserRechargeHistory> userRecharges = new ObservableCollection<UserRechargeHistory>();
                            foreach (UserRechargeHistory userRecharge in dataList[0] as ObservableCollection<UserRechargeHistory>)
                            {
                                userRecharges.Add(userRecharge);
                            }
                            foreach (UserRechargeHistory rechargeRecord in userRecharges)
                            {
                                cvDatabase.UserRechargeHistories.DeleteObject(cvDatabase.UserRechargeHistories.First(x=>x.UserID.Equals(rechargeRecord.UserID) && x.DateWithTime == rechargeRecord.DateWithTime && x.EmployeeID.Equals(rechargeRecord.EmployeeID) ));
                                (this.ResHisGridView.ItemsSource as ObservableCollection<UserRechargeHistory>).Remove(rechargeRecord);
                            }
                        }
                        else
                        {
                            ObservableCollection<TeamRechargeHistory> teamRecharges = new ObservableCollection<TeamRechargeHistory>();
                            foreach (TeamRechargeHistory userRecharge in dataList[0] as ObservableCollection<TeamRechargeHistory>)
                            {
                                teamRecharges.Add(userRecharge);
                            }
                            foreach (TeamRechargeHistory rechargeRecord in teamRecharges)
                            {
                                cvDatabase.TeamRechargeHistories.DeleteObject(cvDatabase.TeamRechargeHistories.First(x=>x.Name.Equals(rechargeRecord.Name) && x.DateWithTime == rechargeRecord.DateWithTime && x.EmployeeID.Equals(rechargeRecord.EmployeeID)));
                                (this.ResHisGridView.ItemsSource as ObservableCollection<TeamRechargeHistory>).Remove(rechargeRecord);
                            }
                        }
                        cvDatabase.SaveChanges();
                    }
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Recharge History Delete All", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.ResHisDeleteAll.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Cutomer Login History

        public ICommand CustLoginHisSearchCommand
        {
            get { return new ReplayCommand(new Action<object>(this.custLoginHisSearchClick)); }
        }

        public ICommand CustLoginHisDeleteCommand
        {
            get { return new ReplayCommand(new Action<object>(this.custLoginHisDeleteClick)); }
        }

        public ICommand CustLoginHisDeleteAllCommand
        {
            get { return new ReplayCommand(new Action<object>(this.custLoginHisDeleteAllClick)); }
        }
        /// <summary>
        /// Search Login History 
        /// </summary>
        /// <param name="obj">
        /// obj[0] = Name
        /// obj[1]= IsName
        /// obj[2] = First Date
        /// obj[3] = Second Date
        /// obj[4] = Third Date
        /// </param>
        private void custLoginHisSearchClick(object obj)
        {
            this.LogHisSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    DateTime firstDate = (DateTime)dataList[2];
                    DateTime secondDate = (DateTime)dataList[3];
                    DateTime thirdDate = (DateTime)dataList[4];
                    IQueryable<UserLoginHistory> searchQuery = null;
                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        if ((bool)dataList[1])
                        {
                            string useName = dataList[0].ToString();
                            switch (this.Option)
                            {
                                case "ByDate":
                                    searchQuery = cvDatabase.UserLoginHistories.Where(x => x.StratTime == firstDate && x.UserID == useName);
                                    break;
                                case "BetweenTwoDate":
                                    searchQuery = cvDatabase.UserLoginHistories.Where(x => x.StratTime == secondDate && x.StratTime == thirdDate && x.UserID == useName);
                                    break;
                                case "All":
                                    searchQuery = cvDatabase.UserLoginHistories.Where(x => x.UserID == useName);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (this.Option)
                            {
                                case "ByDate":
                                    searchQuery = cvDatabase.UserLoginHistories.Where(x => x.StratTime == firstDate);
                                    break;
                                case "BetweenTwoDate":
                                    searchQuery = cvDatabase.UserLoginHistories.Where(x => x.StratTime == secondDate && x.StratTime == thirdDate);
                                    break;
                                case "All":
                                    searchQuery = cvDatabase.UserLoginHistories;
                                    break;
                                default:
                                    break;
                            }
                        }
                        this.LogHisGridView.ItemsSource = new ObservableCollection<ModelUserLogin>(searchQuery.Select(x => new ModelUserLogin {MuniteUses = x.EndTime.Value.Minute-x.StratTime.Minute, CounterNumber=x.CounterNumber,  StratTime=x.StratTime, TeamName=x.TeamName, UserID=x.UserID }));
                    }
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Login History Search", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.LogHisSearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete History
        /// </summary>
        /// <param name="obj"> GridView.SelectedItems </param>
        private void custLoginHisDeleteClick(object obj)
        {
            this.LogHisDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ObservableCollection<ModelUserLogin> loginHistorys = new ObservableCollection<ModelUserLogin>();
                foreach (ModelUserLogin loginHistory in obj as IEnumerable)
                {
                    loginHistorys.Add(loginHistory);
                }
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    foreach (ModelUserLogin loginHistory in loginHistorys)
                    {
                        cvDatabase.UserLoginHistories.DeleteObject(cvDatabase.UserLoginHistories.First(x=>x.StratTime == loginHistory.StratTime && x.UserID.Equals(loginHistory.UserID)));
                        (this.LogHisGridView.ItemsSource as ObservableCollection<ModelUserLogin>).Remove(loginHistory);
                    }
                    cvDatabase.SaveChanges();
                }
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Login History Delete", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.LogHisDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete All History
        /// </summary>
        /// <param name="obj">DataGrid.ItemSource</param>
        private void custLoginHisDeleteAllClick(object obj)
        {
            this.LogHisDeleteAll.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ObservableCollection<ModelUserLogin> userLoginHistorys = new ObservableCollection<ModelUserLogin>();
                foreach (ModelUserLogin loginHistory in userLoginHistorys)
                {
                    userLoginHistorys.Add(loginHistory);
                }
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    foreach (ModelUserLogin loginHistory in userLoginHistorys)
                    {
                        cvDatabase.UserLoginHistories.DeleteObject(cvDatabase.UserLoginHistories.First(x=>EntityFunctions.TruncateTime(x.StratTime) == loginHistory.StratTime && x.UserID.Equals(loginHistory.UserID)));
                        (this.LogHisGridView.ItemsSource as ObservableCollection<ModelUserLogin>).Remove(loginHistory);
                    }
                    cvDatabase.SaveChanges();
                }
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Login History Delete All", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.LogHisDeleteAll.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Cash View and Edit
        public ICommand SearchCashCommand
        {
            get { return new ReplayCommand(new Action<object>(this.searchCashClick)); }
        }

        public ICommand UpdateCashCommand
        {
            get { return new ReplayCommand(new Action<object>(this.updateCashClick)); }
        }
        /// <summary>
        /// Search Cash by Date
        /// </summary>
        /// <param name="obj"> Date time</param>
        private void searchCashClick(object obj)
        {
            this.CashSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait; ;
            try
            {
                DateTime selectedDate = (DateTime)obj;
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    this.CashAmount.Value =Convert.ToDouble(cvDatabase.Cashes.FirstOrDefault(x => x.EntryDate == selectedDate).Amount);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Cash Search", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CashSearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Update Cash 
        /// </summary>
        /// <param name="obj">
        /// obj[0]=dateTime
        /// obj[1]=Amount
        /// </param>
        private void updateCashClick(object obj)
        {
            this.CashUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait; ;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    DateTime selectedDate=(DateTime)dataList[0];
                    double cashAmount= (double)dataList[1];
                    using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                    {
                        var selectedCash = cvDatabase.Cashes.FirstOrDefault(x => x.EntryDate == selectedDate);
                        selectedCash.Amount = Convert.ToDecimal(cashAmount);
                        cvDatabase.SaveChanges();
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,2], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Cash Update", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CashUpdate.IsEnabled = true;
            }
        }
        #region Olde Code
        /// <summary>
        /// Search Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CashViewButtonSearchClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.CashViewButtonSearch.IsEnabled = false;
            //Mouse.OverrideCursor = Cursors.Wait;
            //try
            //{
            //    this.CashViewtextBolockAmount.Text =(from cashtable in new CvDataClassesDataContext().CV_Cashes where cashtable.EntryDate.Equals(this.CashViewDate.SelectedDate) select new { cashtable.Amount }).Single().Amount.ToString();
            //}
            //catch (Exception error)
            //{
            //    Mouse.OverrideCursor = null;
            //    DXMessageBox.Show(error.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //finally
            //{
            //    Mouse.OverrideCursor = null;
            //    this.CashViewButtonSearch.IsEnabled=true;
            //}
        }
        #endregion
        #endregion

        #region Panel Cash History
        public ICommand SearchCashHisotyCommand
        {
            get { return new ReplayCommand(new Action<object>(this.searchCashHistoryClick)); }
        }

        public ICommand PrintCashHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.printCashHistoryClick)); }
        }
        /// <summary>
        /// Search For Cash History
        /// </summary>
        /// <param name="obj">
        /// obj[0]=FirstDate
        /// obj[1] = Second Date
        /// obj[2] = Thread Date
        /// </param>
        private void searchCashHistoryClick(object obj)
        {
            this.CashHistorySearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    IQueryable<Cash> searchQuery = null;
                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        switch (this.Option)
                        {
                            case "ByDate":
                                DateTime firstDate = (DateTime)dataList[0];
                                searchQuery = cvDatabase.Cashes.Where(x => x.EntryDate == firstDate);
                                break;
                            case "BetweenTwoDate":
                                DateTime secondDate=(DateTime)dataList[1];
                                DateTime thirdDate=(DateTime)dataList[2];
                                searchQuery = cvDatabase.Cashes.Where(x => x.EntryDate >= secondDate && x.EntryDate <= thirdDate);
                                break;
                            case "All":
                                searchQuery =cvDatabase.Cashes;
                                break;
                            default:
                                break;
                        }
                        this.CashHistoryGrid.ItemsSource = new ObservableCollection<Cash>(searchQuery.Select(x=>x));
                    }
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Cash History Search", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.CashHistorySearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Print History
        /// </summary>
        /// <param name="obj"></param>
        private void printCashHistoryClick(object obj)
        {
            this.CashHistoryPrint.IsEnabled = false;
            try
            {
                CashHistoryReport newCashHistoryReport = new CashHistoryReport();
                newCashHistoryReport.CashHistoryBindingSource.DataSource = obj as IEnumerable;
                PrintHelper.ShowPrintPreviewDialog(this, newCashHistoryReport);
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Cash History Print", ErrorException);
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                this.CashHistoryPrint.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Business Summary
        public ICommand SearchSummaryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.searchSummaryClick)); }
        }
        /// <summary>
        /// Search Daily Summary
        /// </summary>
        /// <param name="obj">DateTime</param>
        private void searchSummaryClick(object obj) 
        {
            this.SummarySearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DateTime selectedDate = (DateTime)obj;
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    double customerMinutes=0, teamMinutes=0;
                    var cashByDate= cvDatabase.Cashes.FirstOrDefault(x => x.EntryDate == selectedDate);
                    this.SummaryCash.Text = cashByDate != null ? cashByDate.Amount.ToString() : "0";
                    var customerQuery = cvDatabase.UserRechargeHistories.Where( x => x.DateWithTime == selectedDate);
                    var teamQuery = cvDatabase.TeamRechargeHistories.Where(x => x.DateWithTime == selectedDate);
                    if (customerQuery.Count()>0)
                    {
                        customerMinutes = customerQuery.Sum(x => x.Minutes);
                    }
                    if (teamQuery.Count()>0)
                    {
                        teamMinutes = teamQuery.Sum(x => x.Minutes);
                    }
                    this.SummaryMinutes.Text = (customerMinutes + teamMinutes).ToString();
                    this.SummaryTotalLogin.Text = (from totalLogin in cvDatabase.UserLoginHistories where EntityFunctions.TruncateTime(totalLogin.StratTime) == selectedDate select totalLogin).Count().ToString();
                        //cvDatabase.UserLoginHistories.Where(x => x.StratTime == selectedDate).Count().ToString();
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Summary Search", ErrorException);
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.SummarySearch.IsEnabled = true;
            }
        }
        #endregion

        #region Panel New Team

        public ICommand NewTeamCommand
        {
            get { return new ReplayCommand(new Action<object>(this.newTeam_Click)); }
        }

        public ICommand UpdateTeamCommand
        {
            get { return new ReplayCommand(new Action<object>(this.updateTeam_Click)); }
        }

        public ICommand DeleteTeamCommand
        {
            get { return new ReplayCommand(new Action<object>(this.deleteTeam_Click)); }
        }

        public ICommand BrowseTeamCommand
        {
            get { return new ReplayCommand(new Action<object>(this.browseTeam_Click)); }
        }

        public ICommand WebCamTeamCommand
        {
            get { return new ReplayCommand(new Action<object>(this.webCamTeam_Click)); }
        }

        public ICommand TeamMemberAddCommand
        {
            get { return new ReplayCommand(new Action<object>(this.memberAdd_Click)); }
        }

        public ICommand TeamMemberRemoveCommand
        {
            get { return new ReplayCommand(new Action<object>(this.memberRemove_Click)); }
        }

        /// <summary>
        /// Add New Team 
        /// </summary>
        /// <param name="obj"></param>
        private void newTeam_Click(object obj)
        {
            this.TeamNew.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ModelTeamInfo newTeamInfo = new ModelTeamInfo();
                newTeamInfo.TeamMemberList = new ObservableCollection<ModelCommonUse>();
                (this.TeamGridView.ItemsSource as ObservableCollection<ModelTeamInfo>).Add(newTeamInfo);
                this.TeamGridView.Rebind();
                this.TeamGridView.SelectedItem = this.TeamGridView.Items[this.TeamGridView.Items.IndexOf(newTeamInfo)];
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team New Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.TeamNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// Update or insert Team Information
        /// </summary>
        /// <param name="obj"></param>
        private void updateTeam_Click(object obj)
        {
            this.TeamUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    this.TeamName.GetBindingExpression(TextEdit.TextProperty);
                    this.TeamAdminName.GetBindingExpression(TextEdit.TextProperty);
                    this.getValidationError(this.TeamName,this.TeamAdminName);
                    using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                    {
                        ModelTeamInfo selectedTeamInfo = obj as ModelTeamInfo;
                        var teamExist = cvDatabase.Teams.FirstOrDefault(x => x.Name.Equals(selectedTeamInfo.Name));
                        if (teamExist!=null)
                        {
                             Mouse.OverrideCursor = null;
                             if (DXMessageBox.Show(CvVariables.ERROR_MESSAGES[1, 8], CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                             {
                                 teamExist.AdminName = selectedTeamInfo.AdminName;
                                 teamExist.JoinDate = selectedTeamInfo.JoinDate;
                                 teamExist.Logo = selectedTeamInfo.Image;
                                 teamExist.TeamAccount.Minutes = selectedTeamInfo.Minutes;
                                 foreach (var oldMember in cvDatabase.TeamMembers.Where(x => x.Name.Trim().Equals(selectedTeamInfo.Name)))
                                 {
                                     cvDatabase.TeamMembers.DeleteObject(oldMember);
                                 }
                                // teamExist.TeamMembers.Clear();
                                 foreach (var newMember in selectedTeamInfo.TeamMemberList)
                                 {
                                     teamExist.TeamMembers.Add(new TeamMember{UserID = newMember.UserName, AutoInc = default(long), Name = selectedTeamInfo.Name});
                                 }
                                 cvDatabase.SaveChanges();
                                 Mouse.OverrideCursor = null;
                                 DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 2], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                             }
                             else
                             {
                                 return;
                             }
                        }
                        else
                        {
                            Team newTeam = new Team
                                               {
                                                   JoinDate = selectedTeamInfo.JoinDate,
                                                   Logo = selectedTeamInfo.Image,
                                                   Name = selectedTeamInfo.Name,
                                                   AdminName = selectedTeamInfo.AdminName,
                                                   TeamAccount = new TeamAccount { Minutes=selectedTeamInfo.Minutes, Status=false, EntryDate=DateTime.Today }
                                               };
                            foreach (var newMember in selectedTeamInfo.TeamMemberList)
                            {
                                newTeam.TeamMembers.Add(new TeamMember
                                                            {
                                                                AutoInc = default(long),
                                                                UserID = newMember.UserName
                                                            });
                            }
                            cvDatabase.Teams.AddObject(newTeam);
                            cvDatabase.SaveChanges();
                            Mouse.OverrideCursor = null;
                            DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 3], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Update Or Insert Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.TeamUpdate.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete a team information
        /// </summary>
        /// <param name="obj"></param>
        private void deleteTeam_Click(object obj)
        {
            this.TeamDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    ModelTeamInfo deleteedTeam = obj as ModelTeamInfo;
                    using (Cafeteria_Vernier_dbEntities cvDatbase= new Cafeteria_Vernier_dbEntities())
                    {
                        cvDatbase.Teams.DeleteObject(cvDatbase.Teams.First(x=>x.Name.Equals(deleteedTeam.Name)));
                        cvDatbase.SaveChanges();
                        (this.TeamGridView.ItemsSource as ObservableCollection<ModelTeamInfo>).Remove(deleteedTeam);
                        this.TeamGridView.Rebind();
                        this.selectGridViewFirstItem(this.TeamGridView);
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team Delete Click", ErrorException);
                 Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.TeamDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Browse image for a Team
        /// </summary>
        /// <param name="obj"></param>
        private void browseTeam_Click(object obj)
        {
            this.TeamBrowes.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    (obj as ModelTeamInfo).Image = this.selectImageFromFile();
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team Browse Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.TeamBrowes.IsEnabled = true;
            }
        }
        /// <summary>
        /// Snapshot from webcam
        /// </summary>
        /// <param name="obj"></param>
        private void webCamTeam_Click(object obj)
        {
            this.TeamWebcam.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                 if (obj != null)
                {
                    byte[] imageInBytes = null;
                    imageInBytes = WebCamWindow.CaptureImage();
                    Mouse.OverrideCursor = Cursors.Wait;
                    if (imageInBytes != null)
                    {
                        (obj as ModelTeamInfo).Image = imageInBytes;
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team web cam Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.TeamWebcam.IsEnabled = true;
            }
        }
        /// <summary>
        /// Add new Member to the team
        /// </summary>
        /// <param name="obj"></param>
        private void memberAdd_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj !=null)
                {
                    ModelCommonUse newMember = obj as ModelCommonUse;
                    var membersList= this.TeamMemberList.ItemsSource as ObservableCollection<ModelCommonUse>;
                    if (membersList.FirstOrDefault(x=>x.UserName.Trim().Equals(newMember.UserName))==null)
                    {
                        membersList.Add(newMember);
                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show("This user is already a member.", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team member add Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;

            }
        }
        /// <summary>
        /// Remove Member from Member list
        /// </summary>
        /// <param name="obj"></param>
        private void memberRemove_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    (this.TeamMemberList.ItemsSource as ObservableCollection<ModelCommonUse>).Remove(obj as ModelCommonUse);
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team member remove click", ErrorException);
                 Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region Panel User Maintenance
        public ICommand UserMaintenanceSearchCommand
        {
            get { return new ReplayCommand(new Action<object>(this.userMaintenanceSearchClick)); }
        }

        public ICommand UserMiantenanceDeleteCommand
        {
            get { return new ReplayCommand(new Action<object>(this.userMaintenanceDeleteClick)); }
        }

        public ICommand UserMiantenanceDeleteAllCommand
        {
            get { return new ReplayCommand(new Action<object>(this.userMaintenanceDeleteAllClick)); }
        }
        /// <summary>
        /// Search User`s
        /// </summary>
        /// <param name="obj">
        /// obj[0] = Name
        /// obj[1] = FirstDate
        /// obj[2] = Second Date
        /// obj[3] = Third Date
        /// obj[4] = Minutes
        /// </param>
        private void userMaintenanceSearchClick(object obj)
        {
            this.UserMaintenanceSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    IQueryable<CustomerInformation> searchQuery = null;
                    Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities();
                        switch (this.Option)
                        {
                            case "ByName":
                                string username=(string)dataList[0];
                                searchQuery = cvDatabase.CustomerInformations.Where(x => x.UserID == username);
                                break;
                            case "ByDate":
                                switch (this.SubOption)
                                {
                                    case "ByDate":
                                        DateTime joinDateEqual = (DateTime)dataList[1];
                                        searchQuery = cvDatabase.CustomerInformations.Where(x => x.JoinDate == joinDateEqual);
                                        break;
                                    case "Below":
                                        DateTime joinDateBelow = (DateTime)dataList[1];
                                        searchQuery = cvDatabase.CustomerInformations.Where(x => x.JoinDate <= joinDateBelow);
                                        break;
                                    case "TwoDate":
                                        DateTime firstDate = (DateTime)dataList[2];
                                        DateTime seconDate = (DateTime)dataList[3];
                                        searchQuery = cvDatabase.CustomerInformations.Where(x => x.JoinDate <= firstDate && x.JoinDate>=seconDate);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "ByAmount":
                                var minutes = (double)dataList[4];
                                switch (this.SubOption)
                                {
                                    case "Below":
                                        searchQuery = cvDatabase.CustomerInformations.Where(x => x.CustomerAccount.Minutes<=minutes);
                                        break;
                                    case "Equal":
                                        searchQuery = cvDatabase.CustomerInformations.Where(x => x.CustomerAccount.Minutes == minutes);
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        this.UserMaintenanceCutomerGridView.ItemsSource = new ObservableCollection<CustomerInformation>(searchQuery);
                    }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("User Maintenance Search", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.UserMaintenanceSearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Selected User
        /// </summary>
        /// <param name="obj">GridView.SelectedItems</param>
        private void userMaintenanceDeleteClick(object obj)
        {
            this.UserMaintenanceDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    ObservableCollection<CustomerInformation> customerInfos = new ObservableCollection<CustomerInformation>();
                    foreach (var singleItem in obj as IEnumerable)
                    {
                        customerInfos.Add(singleItem as CustomerInformation);
                    }
                    foreach (CustomerInformation singleCutomer in customerInfos)
                    {
                        cvDatabase.CustomerInformations.DeleteObject(cvDatabase.CustomerInformations.First(x=>x.UserID.Equals(singleCutomer.UserID)));
                        (this.UserMaintenanceCutomerGridView.ItemsSource as ObservableCollection<CustomerInformation>).Remove(singleCutomer);
                    }
                    cvDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("User Maintenance Delete", ErrorException);
                DXMessageBox.Show(ErrorException.Message);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.UserMaintenanceDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete All User Info
        /// </summary>
        /// <param name="obj">Gridview.ItemSource</param>
        private void userMaintenanceDeleteAllClick(object obj) 
        {
            this.UserMaintenanceDeleteAll.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    ObservableCollection<CustomerInformation> customerInfos = new ObservableCollection<CustomerInformation>();
                    foreach (var singleItem in obj as ObservableCollection<CustomerInformation>)
                    {
                        customerInfos.Add(singleItem as CustomerInformation);
                    }
                    foreach (CustomerInformation singleCutomer in customerInfos)
                    {
                        cvDatabase.CustomerInformations.DeleteObject(cvDatabase.CustomerInformations.First(x => x.UserID.Equals(singleCutomer.UserID)));
                        (this.UserMaintenanceCutomerGridView.ItemsSource as ObservableCollection<CustomerInformation>).Remove(singleCutomer);
                    }
                    cvDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("User Maintenance All Delete", ErrorException);
                DXMessageBox.Show(ErrorException.Message);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.UserMaintenanceDeleteAll.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Team Maintenance

        public ICommand TeamMainSearchCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamMaintenanceSearch)); }
        }

        public ICommand TeamMainDeleteCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamMaintenanceDelete)); }
        }

        public ICommand TeamMainDeleteAllCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamMaintenanceDeleteAll)); }
        }
        /// <summary>
        /// Search User`s
        /// </summary>
        /// <param name="obj">
        /// obj[0] = Name
        /// obj[1] = FirstDate
        /// obj[2] = Second Date
        /// obj[3] = Third Date
        /// obj[4] = Minutes
        /// </param>
        private void teamMaintenanceSearch(object obj)
        {
            this.temMainSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    IQueryable<Team> searchQuery = null;
                    Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities();
                        switch (this.Option)
                        {
                            case "ByName":
                                string teamName = (string)dataList[0];
                                searchQuery = cvDatabase.Teams.Where(x => x.Name == teamName);
                                break;
                            case "ByDate":
                                switch (this.SubOption)
                                {
                                    case "ByDate":
                                        DateTime joinDateEqual = (DateTime)dataList[1];
                                        searchQuery = cvDatabase.Teams.Where(x => x.JoinDate == joinDateEqual);
                                        break;
                                    case "Below":
                                        DateTime joinDateBelow = (DateTime)dataList[1];
                                        searchQuery = cvDatabase.Teams.Where(x => x.JoinDate <= joinDateBelow);
                                        break;
                                    case "TwoDate":
                                        DateTime firstDate = (DateTime)dataList[2];
                                        DateTime seconDate = (DateTime)dataList[3];
                                        searchQuery = cvDatabase.Teams.Where(x => x.JoinDate <= firstDate && x.JoinDate >= seconDate);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "ByAmount":
                                var minutes = (double)dataList[4];
                                switch (this.SubOption)
                                {
                                    case "Below":
                                        searchQuery = cvDatabase.Teams.Where(x => x.TeamAccount.Minutes <= minutes);
                                        break;
                                    case "Equal":
                                        searchQuery = cvDatabase.Teams.Where(x => x.TeamAccount.Minutes == minutes);
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        this.teamMainGridView.ItemsSource = new ObservableCollection<Team>(searchQuery);
                    }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team Maintenance Search", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.temMainSearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Selected User
        /// </summary>
        /// <param name="obj">GridView.SelectedItems</param>
        private void teamMaintenanceDelete(object obj)
        {
            this.teamMainDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    ObservableCollection<Team> customerInfos = new ObservableCollection<Team>();
                    foreach (var singleItem in obj as IEnumerable)
                    {
                        customerInfos.Add(singleItem as Team);
                    }
                    foreach (Team singleCutomer in customerInfos)
                    {
                        cvDatabase.Teams.DeleteObject(cvDatabase.Teams.First(x=>x.Name.Equals(singleCutomer.Name)));
                        (this.teamMainGridView.ItemsSource as ObservableCollection<Team>).Remove(singleCutomer);
                    }
                    cvDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team Maintenance Delete", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.teamMainDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete All User Info
        /// </summary>
        /// <param name="obj">Gridview.ItemSource</param>
        private void teamMaintenanceDeleteAll(object obj)
        {
            this.teamMainDeleteAll.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    ObservableCollection<Team> customerInfos = new ObservableCollection<Team>();
                    foreach (var singleItem in obj as ObservableCollection<Team>)
                    {
                        customerInfos.Add(singleItem as Team);
                    }
                    foreach (Team singleCutomer in obj as ObservableCollection<Team>)
                    {
                        cvDatabase.Teams.DeleteObject(cvDatabase.Teams.First(x => x.Name.Equals(singleCutomer.Name)));
                        (this.teamMainGridView.ItemsSource as ObservableCollection<Team>).Remove(singleCutomer);
                    }
                    cvDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team Maintenance DeleteAll", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.teamMainDeleteAll.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Send Mail
        public ICommand EmailSendCommand
        {
            get { return new ReplayCommand(new Action<object>(this.emailSend)); }
        }

        public ICommand EmailSendingCancelCommand
        {
            get { return new ReplayCommand(new Action<object>(this.emailSendingCancel)); }
        }
        /// <summary>
        /// obj[0] = Subject
        /// obj[1] = body
        /// obj[2] = Name
        /// </summary>
        /// <param name="obj"></param>
        private void emailSend(object obj)
        {
            this.sendMailSend.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.sendMailWorker.RunWorkerAsync(obj);
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Email Sending", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                this.sendMailSend.IsEnabled = true;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.sendMailCancel.IsEnabled = true;
            }
        }

        private void emailSendingCancel(object obj)
        {
            this.sendMailCancel.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.sendMailWorker.CancelAsync();
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Email Sending Cancel", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                this.sendMailCancel.IsEnabled = true;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.sendMailSend.IsEnabled = true;
            }
        }

        private void sendMailWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                DXMessageBox.Show(e.Error.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (e.Cancelled)
            {
                DXMessageBox.Show("Sending Email Canceled", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                DXMessageBox.Show("Email`s are send successfully", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void sendMailWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.sendMailPersentage.Text = string.Format("{0}%", e.ProgressPercentage);
            this.sendMailProgress.Value = e.ProgressPercentage;
        }

        private void sendMailWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ArrayList dataList = e.Argument as ArrayList;
                List<string> toAddress = new List<string>();
                using (Cafeteria_Vernier_dbEntities cvDatbase = new Cafeteria_Vernier_dbEntities())
                {
                    switch (this.Option)
                    {
                        case "OneByOne":
                            string customerName = dataList[2].ToString();
                            toAddress = cvDatbase.CustomerInformations.Where(x => x.UserID.Equals(customerName) && x.Email != null).Select(x => x.Email).ToList();
                            break;
                        case "EveryOne":
                            toAddress = cvDatbase.CustomerInformations.Where(x => x.Email != null).Select(x => x.Email).ToList();
                            break;
                        default:
                            break;
                    }
                }
                this.Dispatcher.Invoke(new Action(() => this.sendMailProgress.Maximum = toAddress.Count()));
                ProcestaSendMail sendEmails = new ProcestaSendMail();
                string userEmail = Properties.Settings.Default.settingEmail;
                string userPassword = Properties.Settings.Default.settingEmailPassword;
                string emailSubject = dataList[0].ToString();
                string emailBody = dataList[1].ToString();
                for (int i = 0; i < toAddress.Count(); i++)
                {
                    if (this.sendMailWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        sendEmails.SendingMail(userEmail, userPassword, "smtp.gmail.com", 587, toAddress[i], emailSubject, emailBody);
                        this.sendMailWorker.ReportProgress(i);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region panel New Employ
 
        public ICommand NewEmployeeCommand
        {
            get { return new ReplayCommand(new Action<object>(this.newEmployeeClick));}
        }
        public ICommand UpdateEmployeeCommand
        {
            get { return new ReplayCommand(new Action<object>(this.updateEmployeeClick)); }
        }
        public ICommand DeleteEmployeeCommand
        {
            get { return new ReplayCommand(new Action<object>(this.deleteEmployeeClick)); }
        }
        public ICommand ReportEmployeeCommand
        {
            get { return new ReplayCommand(new Action<object>(this.reportEmployeeClick)); }
        }
        public ICommand BrowseEmployeeCommand
        {
            get { return new ReplayCommand(new Action<object>(this.browseEmployeeClick)); }
        }
        public ICommand WebCamEmployeeCommand
        {
            get { return new ReplayCommand(new Action<object>(this.webCamEmployeeClick)); }
        }
        /// <summary>
        /// Add new Employ Information 
        /// </summary>
        /// <param name="obj">In this Case Parameter is not need.</param>
        private void newEmployeeClick(object obj)
        {
            this.EmployeeNew.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ViewEmployPermissions defaultPermission = new ViewEmployPermissions();
                ModelEmployee newEmployee = new ModelEmployee();
                newEmployee.Permissions = new ObservableCollection<ModelEmployPermissions>(defaultPermission.Where(x=>x.SupperTip!=null).OrderBy(x=>x.Priority));
                (this.EmployeeGridView.ItemsSource as ObservableCollection<ModelEmployee>).Add(newEmployee);
                this.EmployeeGridView.Rebind();
                this.EmployeeGridView.SelectedItem = this.EmployeeGridView.Items[this.EmployeeGridView.Items.IndexOf(newEmployee)];
            }
            catch (Exception errorException)
            {
                LogFileWriter.ErrorToLog("New Employee Click",errorException);
                DXMessageBox.Show(errorException.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// Update or Insert Employee Information 
        /// </summary>
        /// <param name="obj">RadGridView selected item</param>
        private void updateEmployeeClick(object obj)
        {
            this.EmployeeUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    ModelEmployee selectedEmployee = obj as ModelEmployee;
                    using (Cafeteria_Vernier_dbEntities CVDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        var employeeExist = CVDatabase.Employees.FirstOrDefault(x => x.EmployeeID.Equals(selectedEmployee.Name));
                        if (employeeExist!=null)
                        {
                            if (this.LoginEmployee.Permissions.AsParallel().FirstOrDefault(x => x.Item.Equals("EmployeeEdit")) != null)
                            {
                                Mouse.OverrideCursor = null;
                                if (DXMessageBox.Show(CvVariables.ERROR_MESSAGES[1, 8], CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    Mouse.OverrideCursor = Cursors.Wait;
                                    var existinfPermissions = CVDatabase.EmployeePermissions.Where(x => x.EmployeeID.Equals(selectedEmployee.Name));
                                    foreach (var permission in existinfPermissions)
                                    {
                                        CVDatabase.EmployeePermissions.DeleteObject(permission);
                                    }
                                    employeeExist.Address = selectedEmployee.Address;
                                    employeeExist.Password = String.IsNullOrEmpty(selectedEmployee.Password) ? employeeExist.Password : selectedEmployee.Password;
                                    employeeExist.Phone = selectedEmployee.PhoneNmber;
                                    employeeExist.UserImage = selectedEmployee.UserImage;
                                    foreach (var permission in selectedEmployee.Permissions.Where(x=>x.Permission.Equals(true)))
                                    {
                                       employeeExist.EmployeePermissions.Add(new EmployeePermission { AutoInc=default(long),  EmployeeID=selectedEmployee.Name, Privilege=permission.Item, SettingPrivilage = permission.Setting });
                                    }
                                    CVDatabase.SaveChanges();
                                    Mouse.OverrideCursor = null;
                                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 2], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                Mouse.OverrideCursor = null;
                                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 1], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                            }
                        }
                        else
                        {
                            Employee newEmployee = new Employee { Address=selectedEmployee.Address, EmployeeID=selectedEmployee.Name, Password=selectedEmployee.Password, Phone=selectedEmployee.PhoneNmber, UserImage=selectedEmployee.UserImage };
                            foreach (var permission in selectedEmployee.Permissions.Where(x=>x.Permission.Equals(true)))
                            {
                                newEmployee.EmployeePermissions.Add(new EmployeePermission { AutoInc=default(long), EmployeeID=selectedEmployee.Name, Privilege=permission.Item, SettingPrivilage=permission.Setting });
                            }
                            CVDatabase.Employees.AddObject(newEmployee);
                            CVDatabase.SaveChanges();
                            Mouse.OverrideCursor = null;
                            DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 3], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                   DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,4],CvVariables.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Hand);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Update Or Insert Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeUpdate.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Employee Information 
        /// </summary>
        /// <param name="obj">RadGridView selected item</param>
        private void deleteEmployeeClick(object obj)
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    if (DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,6],CvVariables.SOFTWARE_NAME,MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                    {
                        this.EmployeeDelete.IsEnabled = false;
                        ModelEmployee deleteEmployee = obj as ModelEmployee;
                        using (Cafeteria_Vernier_dbEntities CVDatabase = new Cafeteria_Vernier_dbEntities())
                        {
                            CVDatabase.Employees.DeleteObject(CVDatabase.Employees.First(x => x.EmployeeID.Equals(deleteEmployee.Name)));
                            CVDatabase.SaveChanges();
                            (this.EmployeeGridView.ItemsSource as ObservableCollection<ModelEmployee>).Remove(deleteEmployee);
                            this.EmployeeGridView.Rebind();
                            this.selectGridViewFirstItem(this.EmployeeGridView);
                        }
                        Mouse.OverrideCursor = null;
                        DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        return;
                    }
                   
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,4],CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Employee Delete Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Make a Report based on all employee information
        /// </summary>
        /// <param name="obj">RadGridView ItemSource </param>
        private void reportEmployeeClick(object obj)
        {
            this.EmployeeReport.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj !=null)
                {
                    ModelEmployee selectedEmployeeReport = obj as ModelEmployee;
                    ReportEmployee reportSelectedEmployee = new ReportEmployee();
                    reportSelectedEmployee.EmployeePermissions.DataSource = new ObservableCollection<ModelEmployPermissions>(from selectedEmployeePermission in selectedEmployeeReport.Permissions.AsParallel() select new ModelEmployPermissions { Item = selectedEmployeePermission.SupperTip, ScreenTip = selectedEmployeePermission.Permission ? "Yes" : "No" });
                    reportSelectedEmployee.EmployeeInfo.DataSource = selectedEmployeeReport;
                    PrintHelper.ShowPrintPreview(this, reportSelectedEmployee);
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Employee Report Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeReport.IsEnabled = true;
            }
        }
        /// <summary>
        /// Set a Image to Employee profile from file.
        /// </summary>
        /// <param name="obj">RadGridView Selected Item</param>
        private void browseEmployeeClick(object obj)
        {
            this.EmployeeBrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj!=null)
                {
                    (obj as ModelEmployee).UserImage = this.selectImageFromFile();
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Employee Browse Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeBrowse.IsEnabled = true;
            }
        }
        /// <summary>
        /// Set a Image to Employee profile from a Web Cam.
        /// </summary>
        /// <param name="obj">RadGridView Selected Item</param>
        private void webCamEmployeeClick(object obj)
        {
            this.EmployeeWebCam.IsEnabled = false;
            try
            {
                if (obj != null)
                {
                    byte[] imageInBytes = null;
                    imageInBytes = WebCamWindow.CaptureImage();
                    Mouse.OverrideCursor = Cursors.Wait;
                    if (imageInBytes!=null)
                    {
                        (obj as ModelEmployee).UserImage=imageInBytes;
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 4], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Employee WebCam Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.EmployeeWebCam.IsEnabled = true;
            }
        }
        /// <summary>
        /// Permission DataGrid CheckAll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeePermissionChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Parallel.ForEach((this.EmployeePermission.ItemsSource as ObservableCollection<ModelEmployPermissions>), currentPermission =>
                    {
                        currentPermission.Permission = true;
                    });
                this.EmployeePermission.Rebind();
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Employee CheckAll Checked", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void EmployeePermissionUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Parallel.ForEach((this.EmployeePermission.ItemsSource as ObservableCollection<ModelEmployPermissions>), currentPermission =>
                {
                    currentPermission.Permission = false;
                }); 
                this.EmployeePermission.Rebind();
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Employee CheckAll Checked", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        #endregion

        #region Panel Database Backup And Restore

        public ICommand BackupLocationCommand
        {
            get { return new ReplayCommand(new Action<object>(this.browseBackupLocation_Click)); }
        }

        public ICommand DatabaseBackupCommand
        {
            get { return new ReplayCommand(new Action<object>(this.databaseBackup_Click)); }
        }

        public ICommand RestoreLocationCommand
        {
            get { return new ReplayCommand(new Action<object>(this.browseRestoreLocation_Click)); }
        }

        public ICommand DatabaseRestore
        {
            get { return new ReplayCommand(new Action<object>(this.databaseRestore_Click)); }
        }

        private void browseBackupLocation_Click(object obj)
        {
            this.dbbackupBrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                System.Windows.Forms.FolderBrowserDialog backupPathDialog = new System.Windows.Forms.FolderBrowserDialog();
                backupPathDialog.Description = "Please Select a folder for backup file";
                backupPathDialog.ShowNewFolderButton = true;
                if (backupPathDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
                {
                    this.dbbackupFolderPath.Text = backupPathDialog.SelectedPath;
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Database backup folder browse Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.dbbackupBrowse.IsEnabled = true;
            }
        }

        private void databaseBackup_Click(object obj)
        {
            this.dbbackupBackup.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            this.dbBackupWorker.RunWorkerAsync(obj);
        }

        private void browseRestoreLocation_Click(object obj)
        {
            this.dbrestoreBrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                System.Windows.Forms.OpenFileDialog restorePathDialog = new System.Windows.Forms.OpenFileDialog();
                restorePathDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                restorePathDialog.Filter = "SQL Backup file (*.bak)|*.bak";
                if (restorePathDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
                {
                    this.dbrestoreFilePath.Text = restorePathDialog.FileName;
                    FileInfo selectedFileInfo = new FileInfo(restorePathDialog.FileName);
                    this.dbRestoreDate.Text = selectedFileInfo.CreationTime.ToString();
                }
            }
           catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Database restore file browse Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.dbrestoreBrowse.IsEnabled = true;
            }
        }

        private void databaseRestore_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.dbrestoreRestore.IsEnabled = false;
            this.dbRestoreWorker.RunWorkerAsync(obj);
        }

        private void dbRestoreWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            if (e.Error != null)
            {
                DXMessageBox.Show(e.Error.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 8], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.dbrestoreRestore.IsEnabled = true;
        }

        void dbRestoreWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage==0)
            {
                this.dbRestoreProgress.IsIndeterminate = true;
            }
            else if (e.ProgressPercentage==1)
            {
                this.dbRestoreProgress.IsIndeterminate = false;
            }
            else
            {
                this.dbRestoreProgress.Value = e.ProgressPercentage;
            }
        }

        void dbRestoreWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (SqlConnection restoreConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True"))
                {
                    restoreConnection.Open();
                    this.dbRestoreWorker.ReportProgress(10);
                    System.Threading.Thread.Sleep(200);
                    SqlCommand UseMasterCommand = new SqlCommand("USE master", restoreConnection);
                    UseMasterCommand.ExecuteNonQuery();
                    string Alter1 = string.Format(@"ALTER DATABASE [{0}] SET Single_User WITH Rollback Immediate", CvVariables.Catalog);
                    SqlCommand Alter1Cmd = new SqlCommand(Alter1, restoreConnection);
                    Alter1Cmd.ExecuteNonQuery();
                    Alter1Cmd.Dispose();
                    this.dbRestoreWorker.ReportProgress(20);
                    System.Threading.Thread.Sleep(200);
                    this.dbRestoreWorker.ReportProgress(0);
                    string Restore = string.Format(@"RESTORE DATABASE [{0}] FROM DISK = N'{1}' WITH  FILE = 1,  NOUNLOAD,  STATS = 10,MOVE '{0}' TO " + @"'{2}\{0}.mdf',MOVE '{0}_log' TO '{2}\{0}_log.ldf'", CvVariables.Catalog, e.Argument.ToString(), Properties.Settings.Default.SqlDataFolder);
                    SqlCommand RestoreCmd = new SqlCommand(Restore, restoreConnection);
                    RestoreCmd.ExecuteNonQuery();
                    RestoreCmd.Dispose();
                    this.dbRestoreWorker.ReportProgress(1);
                    string Alter2 = string.Format(@"ALTER DATABASE [{0}] SET Multi_User", CvVariables.Catalog);
                    SqlCommand Alter2Cmd = new SqlCommand(Alter2, restoreConnection);
                    Alter2Cmd.ExecuteNonQuery();
                    Alter2Cmd.Dispose();
                    System.Threading.Thread.Sleep(200);
                    this.dbRestoreWorker.ReportProgress(30);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        void dbBackupWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            if (e.Error!=null)
            {
                DXMessageBox.Show(e.Error.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,7], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.dbbackupBackup.IsEnabled = true;
        }

        void dbBackupWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage==0)
            {
                this.dbBackupProgress.IsIndeterminate = true;
            }
            else if (e.ProgressPercentage==1)
            {
                this.dbBackupProgress.IsIndeterminate = false;
            }
            else
            {
                this.dbBackupProgress.Value = e.ProgressPercentage;
            }
        }

        void dbBackupWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (SqlConnection backupConnection = new SqlConnection(Properties.Settings.Default.Cafeteria_Vernier_dbConnectionString))
                {
                    backupConnection.Open();
                    dbBackupWorker.ReportProgress(10);
                    System.Threading.Thread.Sleep(200);
                    ServerConnection sc = new ServerConnection(backupConnection);
                    Server databaseServer = new Server(sc);
                    Backup databaseBackup = new Backup();
                    databaseBackup.Action = BackupActionType.Database;
                    dbBackupWorker.ReportProgress(20);
                    System.Threading.Thread.Sleep(200);
                    databaseBackup.Database = backupConnection.Database.ToString();
                    databaseBackup.Devices.Add(new BackupDeviceItem(System.IO.Path.Combine(e.Argument.ToString(), string.Format("{0}.bak", DateTime.Now.ToString("ddMMyyyyhhsstt"))), DeviceType.File));
                    databaseBackup.LogTruncation = BackupTruncateLogType.Truncate;
                    dbBackupWorker.ReportProgress(0);
                    databaseBackup.SqlBackup(databaseServer);
                    dbBackupWorker.ReportProgress(1);
                    System.Threading.Thread.Sleep(200);
                    sc.ForceDisconnected();
                    dbBackupWorker.ReportProgress(30);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        #region old code
        /// <summary>
        /// Restore Button Browse Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restoreButtonBrowseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.restoreButtonBrowse.IsEnabled = false;
            //Mouse.OverrideCursor = Cursors.Wait; ;
            //try
            //{
            //    System.Windows.Forms.OpenFileDialog restorePathDialog = new System.Windows.Forms.OpenFileDialog();
            //    restorePathDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //    restorePathDialog.Filter = "SQL BackUp file (*.bak)|*.bak"; 
            //    if (restorePathDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            //    {
            //        this.restoreTextboxPath.Text = restorePathDialog.FileName;
            //    }

            //}
            //catch (Exception error)
            //{
            //    Mouse.OverrideCursor = null;
            //    DXMessageBox.Show(error.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //finally
            //{
            //    Mouse.OverrideCursor = null;
            //    this.restoreButtonBrowse.IsEnabled = true;
            //}
        }
        /// <summary>
        /// Restore Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restoreButtonRestoreClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.restoreButtonRestore.IsEnabled = false;
            //Mouse.OverrideCursor = Cursors.Wait; ;
            //try
            //{
            //    cvTimer.Stop();
            //    using (SqlConnection restoreConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True"))
            //    {
            //        restoreConnection.Open();
            //        SqlCommand UseMasterCommand = new SqlCommand("USE master", restoreConnection);
            //        UseMasterCommand.ExecuteNonQuery();
            //        string Alter1 = string.Format(@"ALTER DATABASE [{0}] SET Single_User WITH Rollback Immediate", CvVariables.Catalog);
            //        SqlCommand Alter1Cmd = new SqlCommand(Alter1, restoreConnection);
            //        Alter1Cmd.ExecuteNonQuery();
            //        Alter1Cmd.Dispose();
            //        string Restore = string.Format(@"RESTORE DATABASE [{0}] FROM DISK = N'{1}' WITH  FILE = 1,  NOUNLOAD,  STATS = 10,MOVE '{0}' TO " + @"'{2}\{0}.mdf',MOVE '{0}_log' TO '{2}\{0}_log.ldf'", CvVariables.Catalog, this.restoreTextboxPath.Text, Properties.Settings.Default.SqlDataFolder);
            //        SqlCommand RestoreCmd = new SqlCommand(Restore, restoreConnection);
            //        RestoreCmd.ExecuteNonQuery();
            //        RestoreCmd.Dispose();
            //        string Alter2 = string.Format(@"ALTER DATABASE [{0}] SET Multi_User", CvVariables.Catalog);
            //        SqlCommand Alter2Cmd = new SqlCommand(Alter2, restoreConnection);
            //        Alter2Cmd.ExecuteNonQuery();
            //        Alter2Cmd.Dispose();
            //        cvTimer.Start();
            //        throw new SuccessfullExceptions(CvVariables.ERROR_MESSAGES[2, 6]);
            //    }
            //}
            //catch (SuccessfullExceptions success)
            //{
            //    Mouse.OverrideCursor = null;
            //    DXMessageBox.Show(success.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //catch (Exception error)
            //{
            //    Mouse.OverrideCursor = null;
            //    DXMessageBox.Show(error.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //finally
            //{
            //    Mouse.OverrideCursor = null;
            //    this.restoreButtonRestore.IsEnabled = true;
            //}
        }
        /// <summary>
        /// Backup button browse Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backupButtonBrowseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.backupButtonBrowse.IsEnabled = false;
            //Mouse.OverrideCursor = Cursors.Wait; ;
            //try
            //{
            //    System.Windows.Forms.FolderBrowserDialog backupPathDialog = new System.Windows.Forms.FolderBrowserDialog();
            //    backupPathDialog.Description = "Select a folder for Backup";
            //    backupPathDialog.ShowNewFolderButton = true;
            //    if (backupPathDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            //    {
            //        this.backupTextboxPath.Text = backupPathDialog.SelectedPath;
            //    }
            //}
            //catch (Exception error)
            //{
            //    Mouse.OverrideCursor = null;
            //    DXMessageBox.Show(error.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //finally
            //{
            //    Mouse.OverrideCursor = null;
            //    this.backupButtonBrowse.IsEnabled = true;
            //}
        }
        /// <summary>
        /// Backup Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backupButtonBackupClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.backupButtonBackup.IsEnabled = false;
            //Mouse.OverrideCursor = Cursors.Wait; ;
            //try
            //{
            //    using (SqlConnection backupConnection = new SqlConnection(Properties.Settings.Default.Cafeteria_Vernier_dbConnectionString))
            //    {
            //        backupConnection.Open();
            //        ServerConnection sc = new ServerConnection(backupConnection);
            //        Server databaseServer = new Server(sc);
            //        Backup databaseBackup = new Backup();
            //        databaseBackup.Action = BackupActionType.Database;
            //        databaseBackup.Database = backupConnection.Database.ToString();
            //        if (this.backupTextboxPath.Text.Equals(string.Empty))
            //        {
            //            this.backupButtonBrowseClick(null, null);
            //        }
            //        databaseBackup.Devices.Add(new BackupDeviceItem(new NecessaryFunction().MsSqlBackupFileName(this.backupTextboxPath.Text), DeviceType.File));
            //        databaseBackup.LogTruncation = BackupTruncateLogType.Truncate;
            //        databaseBackup.SqlBackup(databaseServer);
            //        sc.ForceDisconnected();
            //        backupConnection.Dispose();
            //        throw new SuccessfullExceptions(CvVariables.ERROR_MESSAGES[2, 5]);
            //    }
            //}
            //catch (SuccessfullExceptions success)
            //{
            //    Mouse.OverrideCursor = null;
            //    DXMessageBox.Show(success.Message, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //catch (Exception error)
            //{
            //    Mouse.OverrideCursor = null;
            ////    System.IO.File.WriteAllText(@"WriteText.txt", error.InnerException.ToString());
            //    DXMessageBox.Show(error.Message+ error.InnerException, CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //finally
            //{
            //    Mouse.OverrideCursor = null;
            //    this.backupButtonBackup.IsEnabled = true;
            //}
        }
        #endregion
        #endregion

        #region User Status Reset
        public ICommand UserStatusResetCommand
        {
            get { return new ReplayCommand(new Action<object>(this.userStatusReset_Click)); }
        }

        public ICommand TeamStatusResetCommand
        {
            get { return new ReplayCommand(new Action<object>(this.teamStatusReset_Click)); }
        }

        private void userStatusReset_Click(object obj)
        {
            this.userResButtonUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                CustomerAccount userStatus = obj as CustomerAccount;
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    cvDatabase.CustomerAccounts.First(x => x.UserID.Equals(userStatus.UserID)).Status = false;
                    cvDatabase.SaveChanges();
                    this.mainMenuClick("CustomerStatusReset");
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show("Customer Status reset successfully", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("User Reset Update Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.userResButtonUpdate.IsEnabled = true;
            }
        }

        private void teamStatusReset_Click(object obj)
        {
            this.TeamResButtonUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TeamAccount userStatus = obj as TeamAccount;
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    cvDatabase.TeamAccounts.First(x => x.Name.Equals(userStatus.Name)).Status = false;
                    cvDatabase.SaveChanges();
                    this.mainMenuClick("CustomerStatusReset");
                    Mouse.OverrideCursor = null;
                    DXMessageBox.Show("Team Status reset successfully", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Team Reset Update Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.TeamResButtonUpdate.IsEnabled = true;
            }
        }
     
        #endregion

        #region Panel Setting

        public ICommand SettingMenuCommand
        {
            get { return new ReplayCommand(new Action<object>(this.settingMenu_Click)); }
        }

        private void settingMenu_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            IEnumerable<Grid> settingPanels = this.PanelSetting.Children.OfType<Grid>().Where(x => x.Visibility == Visibility.Visible);
            foreach (Grid panel in settingPanels)
            {
                panel.Visibility = Visibility.Hidden;
            }
            switch (obj.ToString())
            {
                case "RateSetting":
                    this.SettingPanelRateSetup.Visibility = Visibility.Visible;
                    break;
                case "ChangePassword":
                    this.settingChangeOldPassword.Password = string.Empty;
                    this.settingChangeNewPassword.Password = string.Empty;
                    this.settingChnageConPassword.Password = string.Empty;
                    this.settingPanelChangePassword.Visibility = Visibility.Visible;
                    break;
                case "ScreenCapture":
                    this.SettingPanelScreenCapture.Visibility = Visibility.Visible;
                    break;
                case "EMailSetting":
                    this.SettingPanelEmailSetting.Visibility = Visibility.Visible;
                    break;
                case "GeneralSetting":
                    this.settingGeneralCheckSystemStart.IsChecked = this.CheckStartupValue();
                    this.SettingPanelGeneral.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            Mouse.OverrideCursor = null;
        }

        #region Panel Minutes Setting

        public ICommand RateSettingNewCommand
        {
            get { return new ReplayCommand(new Action<object>(this.rateSettingNew_Click)); }
        }

        public ICommand RateSettingUpdateCommand
        {
            get { return new ReplayCommand(new Action<object>(this.rateSettingUpdate_Click)); }
        }

        public ICommand RateSettingDeleteCommand
        {
            get { return new ReplayCommand(new Action<object>(this.rateSettingDelete_Click)); }
        }

        private void rateSettingNew_Click(object obj)
        {
            this.RateSettingNew.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ModelBillConfig newRateInfo = new ModelBillConfig();
                (this.RateSettingsGrid.ItemsSource as ObservableCollection<ModelBillConfig>).Add(newRateInfo);
                this.RateSettingsGrid.SelectedItem = this.RateSettingsGrid.Items[this.RateSettingsGrid.Items.IndexOf(newRateInfo)];
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Rate Setup New Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.RateSettingNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// Update Rate Configuration File
        /// </summary>
        /// <param name="obj">Grid Panel DataContext</param>
        private void rateSettingUpdate_Click(object obj)
        {
            this.RateSettingUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.updateRateXml(this.BillConfigInfo);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0,2], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Rate Setup Update Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.RateSettingUpdate.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Rate 
        /// </summary>
        /// <param name="obj">
        /// ComboBox Selected Item (Minutes)
        /// </param>
        private void rateSettingDelete_Click(object obj)
        {
            this.RateSettingDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.BillConfigInfo.Remove(obj as ModelBillConfig);
                this.updateRateXml(this.BillConfigInfo);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 5], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Rate Setup Delete Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.RateSettingDelete.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Change Password
        public ICommand PasswordChangeCommand
        {
            get{return  new ReplayCommand(new Action<object>(this.passwordChange_Click));}
        }

        private void passwordChange_Click(object obj)
        {
            this.settingChageUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    var employeeInfo = cvDatabase.Employees.Where(x => x.EmployeeID.Equals(LoginEmployee.Name) && x.Password.Equals(LoginEmployee.Password)).First();
                    employeeInfo.Password = obj.ToString();
                    cvDatabase.SaveChanges();
                    this.LoginEmployee.Password = obj.ToString();
                }
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 9], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Password change Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.settingChageUpdate.IsEnabled = true;
            }
        }
        #endregion

        #region Panel screen Capture
        public ICommand ScrSnapFolderBrowseCommand
        {
            get { return new ReplayCommand(new Action<object>(this.scrSnapFolderBrowse_Click)); }
        }

        public ICommand ScrSnapStratCommand
        {
            get { return new ReplayCommand( new Action<object>(this.scrSnapStart_Click));}
        }

        public ICommand ScrSnapStopCommand
        {
            get { return new ReplayCommand(new Action<object>(this.scrSnapStop_Click)); }
        }

        private void scrSnapFolderBrowse_Click(object obj)
        {
            this.settingScrCapBrowse.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (System.Windows.Forms.FolderBrowserDialog folderBrowse = new System.Windows.Forms.FolderBrowserDialog())
                {
                    if (folderBrowse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (folderBrowse.SelectedPath.StartsWith("C:\\"))
                        {
                            Mouse.OverrideCursor = null;
                            DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 10], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                        Properties.Settings.Default.schreenCapturePath = folderBrowse.SelectedPath;
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Screen snapshot browse Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.settingScrCapBrowse.IsEnabled = true;
            }
        }

        private void scrSnapStart_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Properties.Settings.Default.CuptureTime = (TimeSpan)obj;
                Properties.Settings.Default.IsCupture = true;
                Properties.Settings.Default.Save();
                ScreenShort.start();
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Screen snapshot start Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void scrSnapStop_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Properties.Settings.Default.IsCupture = false;
                Properties.Settings.Default.Save();
                ScreenShort.Stop();
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Screen snapshot stop Click", ErrorException);
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region Panel General
        /// <summary>
        /// Start with System Start CheckBox Checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingGeneralCheckSystemStartChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true).SetValue("CvServer", System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
        }
        /// <summary>
        /// Start with System Start CheckBox UnChecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingGeneralCheckSystemStartUnChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.CheckStartupValue())
            {
                using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (regKey != null)
                    {
                        regKey.DeleteValue("CvServer");
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private void selectGridViewFirstItem(RadGridView radGridView)
        {
            if (radGridView.Items.Count>0)
            {
                radGridView.SelectedItem = radGridView.Items[0];
            }
        }

        private void selectListBoxFirstItem(ListBox listBox)
        {
            if (listBox.Items.Count>0)
            {
                listBox.SelectedIndex = 0;
            }
        }

        private byte[] selectImageFromFile()
        {
            byte[] imageInbytes = null;
            using (System.Windows.Forms.OpenFileDialog imageOpenBox = new System.Windows.Forms.OpenFileDialog())
            {
                imageOpenBox.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                imageOpenBox.Filter = "JPGE (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|All (*.*)|*.*";
                imageOpenBox.FilterIndex = 0;
                imageOpenBox.RestoreDirectory = true;
                if (imageOpenBox.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
                {
                    imageInbytes = new MiraculousMethods().imageToByteArray(imageOpenBox.FileName);
                }
            }
            return imageInbytes;
        }

        private void getValidationError(params DependencyObject[] dp)
        {
            foreach (DependencyObject depenency in dp)
            {

                foreach (ValidationError errors in Validation.GetErrors(depenency))
                {
                    throw new Exception(errors.ErrorContent.ToString());
                }
            }
        }

        private void hidePanels()
        {
            var visiablePanels = PanelRoot.Children.OfType<Grid>().Where(x => x.Visibility.Equals(Visibility.Visible));
            foreach (var panel in visiablePanels)
            {
                panel.Visibility = Visibility.Hidden;
            }
        }

        private ObservableCollection<ModelCommonUse> customerInfo()
        {
            ObservableCollection<ModelCommonUse> customerShortInfo;
            using (Cafeteria_Vernier_dbEntities cvDatabse = new Cafeteria_Vernier_dbEntities())
            {
                 customerShortInfo = new ObservableCollection<ModelCommonUse>(cvDatabse.CustomerInformations.Select(x =>
                                                                                                        new ModelCommonUse
                                                                                                        {
                                                                                                            UserName = x.UserID,
                                                                                                            Image = x.Logo
                                                                                                        }));
            }
            return customerShortInfo;
        }

        private void updateRateXml(ObservableCollection<ModelBillConfig> bilConfigs)
        {
              string billXmlPath = System.IO.Path.Combine(new MiraculousMethods().GetTempFolder(CvVariables.SOFTWARE_NAME), CvVariables.MUNITIES_FILE);
              if (!File.Exists(billXmlPath))
              {
                  XDocument newRateConfig = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("Rates"));
                  newRateConfig.Save(billXmlPath);
              }
              XElement loadRateConfig = XElement.Load(billXmlPath);
              loadRateConfig.Elements("Rate").Remove();
              foreach (ModelBillConfig rateInfo in bilConfigs)
              {
                  loadRateConfig.Add(new XElement("Rate", new XElement("Minutes", rateInfo.Minutes), new XElement("Bill", rateInfo.Amount)));
              }
              loadRateConfig.Save(billXmlPath);
        }

        #region Methods
        /// <summary>
        /// On Closing 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            MessageBoxResult messBoxResult = DXMessageBox.Show(CvVariables.ERROR_MESSAGES[0, 4], CvVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messBoxResult.Equals(MessageBoxResult.Yes))
            {
                ScreenShort.Stop();
                base.OnClosing(e); 
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Check CvServer Value Exit or Not
        /// </summary>
        /// <returns></returns>
        public bool CheckStartupValue()
        {
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (regKey != null)
                {
                    object keyValue = regKey.GetValue("CvServer");
                    if (keyValue!=null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                
            }
        }
        #endregion
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

        #region ICommand Class
        public class ReplayCommand : ICommand
        {
            private Action<object> _action;

            public ReplayCommand(Action<object> action)
            {
                this._action = action;
            }
            public bool CanExecute(object parameter)
            {
                return true;
            }

            #pragma warning disable 67
            public event EventHandler CanExecuteChanged;
            #pragma warning restore 67

            public void Execute(object parameter)
            {
                try
                {
                    if (parameter != null)
                    {
                        this._action(parameter);
                    }
                    else
                    {
                        DXMessageBox.Show(CvVariables.ERROR_MESSAGESS[0, 0], CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }
                catch (Exception ex)
                {
                    LogFileWriter.ErrorToLog("Replay Command", ex);
                    DXMessageBox.Show(ex.Message,CvVariables.SOFTWARE_NAME,MessageBoxButton.OK,MessageBoxImage.Error);
                }

            }
        }
        #endregion
       
    }
}
