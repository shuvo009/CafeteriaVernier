using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace Procesta.CvServer
{
    class ViewEmployPermissions : ObservableCollection<ModelEmployPermissions>
    {
        public ViewEmployPermissions()
        {
            this.Add(new ModelEmployPermissions { Item = "NewCustomer", Setting = null, Permission = true, Priority = 2, SupperTip = "Create Customer Account", ScreenTip = "Add a new customer", KeboardShortcut = "Ctrl + C", ImagePath = "/ButtonImages/Customer.png" });
            this.Add(new ModelEmployPermissions { Item = "EditCustomer", Setting = null, Permission = false, Priority = 0, SupperTip = "Edit Customer Profile", ScreenTip = null, KeboardShortcut = null, ImagePath = null });
            this.Add(new ModelEmployPermissions { Item = "AccountRecharge", Setting = null, Permission = true, Priority = 5, SupperTip = "Account Recharge", ScreenTip = "Recharge customers account.", KeboardShortcut = "Ctrl + R", ImagePath = "/ButtonImages/Recharge.png" });
            this.Add(new ModelEmployPermissions { Item = "EditRecharge", Setting = null, Permission = false, Priority = 0, SupperTip = "Edit Account Balance", ScreenTip = null, KeboardShortcut = null, ImagePath = null });
            this.Add(new ModelEmployPermissions { Item = "RechargeHistory", Setting = null, Permission = true, Priority = 9, SupperTip = "View Recharge History", ScreenTip = "View customers account recharge history", KeboardShortcut = "Ctrl + E", ImagePath = "/ButtonImages/RechargeHistory.png" });
            this.Add(new ModelEmployPermissions { Item = "CustomerLoginHistory", Setting = null, Permission = true, Priority = 10, SupperTip = "View Login History", ScreenTip = "View customers login history", KeboardShortcut = "Ctrl + L", ImagePath = "/ButtonImages/UserLoginHistory.png" });
            this.Add(new ModelEmployPermissions { Item = "Cash", Setting = null, Permission = true, Priority = 6, SupperTip = "Cash", ScreenTip = "View Cash", KeboardShortcut = "Ctrl + A", ImagePath = "/ButtonImages/CashView.png" });
            this.Add(new ModelEmployPermissions { Item = "EditCash", Setting = null, Permission = false, Priority = 0, ScreenTip = "Edit Cash", SupperTip = "Edit Cash Info", KeboardShortcut = null, ImagePath = null });
            this.Add(new ModelEmployPermissions { Item = "Summary", Setting = null, Permission = true, Priority = 7, SupperTip = "View Business Summary", ScreenTip = "View business summary by date", KeboardShortcut = "Ctrl + S", ImagePath = "/ButtonImages/TodaySummary.png" });
            this.Add(new ModelEmployPermissions { Item = "Setting", Setting = "RateSetting", Permission = false, Priority = 19, SupperTip = "Rate Setup", ScreenTip = "Setup your rate", KeboardShortcut = "Ctrl + U", ImagePath = "/ButtonImages/MoneySetting.png" });
            this.Add(new ModelEmployPermissions { Item = "Setting", Setting = null, Permission = true, Priority = 17, SupperTip = "Setting", ScreenTip = "Application setting", KeboardShortcut = "Ctrl + I", ImagePath = "/ButtonImages/settings.png" });
            this.Add(new ModelEmployPermissions { Item = "CustomerMaintenance", Setting = null, Permission = false, Priority = 11, SupperTip = "Customer Account Maintenance", ScreenTip = "Maintenance your customer`s account`s", KeboardShortcut = "Ctrl + O", ImagePath = "/ButtonImages/userMaintenance.png" });
            this.Add(new ModelEmployPermissions { Item = "TeamMaintenance", Setting = null, Permission = false, Priority = 12, SupperTip = "Team Account Maintenance", ScreenTip = "Maintenance team`s account`s", KeboardShortcut = "Ctrl + N", ImagePath = "/ButtonImages/TeamMaintenance.png" });
            this.Add(new ModelEmployPermissions { Item = "NewTeam", Setting = null, Permission = true, Priority = 3, SupperTip = "Create Team Account", ScreenTip = "Add New Team", KeboardShortcut = "Ctrl + M", ImagePath = "/ButtonImages/Team.png" });
            this.Add(new ModelEmployPermissions { Item = "Setting", Setting = "ChangePassword", Permission = true, Priority = 18, SupperTip = "Change Password", ScreenTip = "Change your Password", KeboardShortcut = "Ctrl + P", ImagePath = "/ButtonImages/ChangePassword.png" });
            this.Add(new ModelEmployPermissions { Item = "Setting", Setting = "ScreenCapture", Permission = false, Priority = 21, SupperTip = "Screen Snapshot", ScreenTip = "Capture host screen.", KeboardShortcut = "Ctrl + F", ImagePath = "/ButtonImages/ScreenCaptureSetting.png" });
            this.Add(new ModelEmployPermissions { Item = "SendEmail", Setting = null, Permission = true, Priority = 13, SupperTip = "Send E-Mail", ScreenTip = "Send E-Mail to your client`s", KeboardShortcut = "Ctrl + J", ImagePath = "/ButtonImages/SenEmail.png" });
            this.Add(new ModelEmployPermissions { Item = "Setting", Setting = "EMailSetting", Permission = false, Priority = 20, SupperTip = "E-Mail Setup", ScreenTip = "Change your E-Mail.", KeboardShortcut = "Ctrl + K", ImagePath = "/ButtonImages/EmailSetting.png" });// Image path need
            this.Add(new ModelEmployPermissions { Item = "Setting", Setting = "GeneralSetting", Permission = true, Priority = 17, SupperTip = "General Setting", ScreenTip = "Setup application general setting", KeboardShortcut = "Ctrl + G", ImagePath = "/ButtonImages/General.png" });
            this.Add(new ModelEmployPermissions { Item = "CountersInformation", Setting = null, Permission = true, Priority = 1, SupperTip = "View Counter`s Information", ScreenTip = "Counter`s Information", KeboardShortcut = "Ctrl + H", ImagePath = "/ButtonImages/Home.png" });
            this.Add(new ModelEmployPermissions { Item = "Setting", Setting = "CounterSetting", Permission = false, Priority = 21, SupperTip = "Counter Setting", ScreenTip = "Setup tour counters", KeboardShortcut = "Ctrl + T", ImagePath = "/ButtonImages/counterSetting.png" });
            this.Add(new ModelEmployPermissions { Item = "CashHistory", Setting = null, Permission = true, Priority = 8, SupperTip = "Cash History", ScreenTip = "See cash history", KeboardShortcut = "Ctrl + Y", ImagePath = "/ButtonImages/CashHHistory.png" });
            this.Add(new ModelEmployPermissions { Item = "NewEmployee", Setting = null, Permission = false, Priority = 4, SupperTip = "Create Employee Account", ScreenTip = "Add new employee", KeboardShortcut = "Ctrl + Q", ImagePath = "/ButtonImages/Employee.png" });
            this.Add(new ModelEmployPermissions { Item = "EmployeeEdit", Setting = null, Permission = false, Priority = 0, SupperTip = "Edit Employee Profile", ScreenTip = null, KeboardShortcut = null, ImagePath = null });
            this.Add(new ModelEmployPermissions { Item = "Database", Setting = null, Permission = false, Priority = 15, SupperTip = "Database Operation", ScreenTip = "Backup and Restore your database.", KeboardShortcut = "Ctrl + D", ImagePath = "/ButtonImages/database.png" });
            this.Add(new ModelEmployPermissions { Item = "CustomerStatusReset", Setting = null, Permission = true, Priority = 16, SupperTip = "Login Status Reset", ScreenTip = "Reset your customer login status", KeboardShortcut = "Ctrl + S", ImagePath = "/ButtonImages/UserReset.png" });
        }
    }
}
