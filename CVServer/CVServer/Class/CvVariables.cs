using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesta.CvServer
{
    public class CvVariables
    {
        public const string Catalog = "Cafeteria_Vernier_db";
        public const string SOFTWARE_NAME = "Cafeteria Vernier";
        public const string MUNITIES_FILE = "BillConfig.xml";
        public const string COUNTER_CONFIG = "countConfig.xml";
        public static string SQL_SERVER_NAME = @".\SQLExpress";
        public const string DEFAULT_ERROR_MESSAGES = "Unwanted error.";
        public const string DEFULT_ERROR_FORMATE = "{0} is required.";

        public static String[,] ERROR_MESSAGES = new String[,] 
        {
            {"Cafeteria Vernier","Fields Are Empty.","Login Failed\nUsername or password may wrong.","Error ! While building menu.","Are you sure you want to Exit ?","Error ! While converting","Logout ! are you sure ?","Password does not match","Weak","Medium","Strong"},
            //      00                  01                      02                                               03                              04                            05                    06                      07            08       09      010
            {"Image size must be less than 800 KB","Username is not available !","Registration successfully done ! ","User not found","Update successfully done !","Error ! \n Please try again.","Deleted successfully !","File not found !","Please select a option.","Successfully added !","Please select at least one userID"},
            //     10                                           11                          12                              13                  14                              15                          16                  17                      18                     19                       110
            {"E-Mail`s has been sent !","Configuration Finished at","Configuration Unfinished at","successfully done","Access denied","Database backup finished.","Database restore finished.","Are you sure you want to overwrite database ?","Member already exits.","Invalid phone number.","Invalid E-mail address."},
            // 20                                   21                      22                      23                      24                  25                      26                                       27                                    28                     29                        210
            {"Team is not available !","Invalid IP address","","","","","","","","",""}
            //       30                         31
        };

        public static string[,] ERROR_MESSAGESS = new string[,]
        {
            {"Invalid Command","Access permission required.","Update successfully done.","Registration successfully done.","Please select an user.","Deleted successfully !","Are you sure are you want to delete ?","Database successfully backup.","Database successfully restore.","Password change successfully.","Please select another place."}
            //      00                  01                              02                              03                          04                      05                          06                                      07                              08                              09                                  010
        };

        public static String[] MENU_PERMISSION = new String[] { "New Customer", "Customer Edit", "Recharge Account", "Recharge Edit", "Recharge History", "User Login History", "Cash", "Cash Edit", "Summary", "Setting", "MoneySetting", "User Maintenance", "Team Maintenance", "New Team", "Change Password", "Screen Capture", "Send Email", "Email Setting", "General Setting", "Home", "Counter Setting", "Counter Config", "New Employ", "Employ Edit", "Database Backup", "UserStatusReset" };
        //                                                             0                1                   2               3               4                   5                   6        7           8          9           10              11                      12            13           14                 15                16             17                  18           19           20                 21              22          23                  24                  25
    }
}
