using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesta.CVClient.Class
{
   public class CVsVariables
    {
       public static string SOTWARE_NAME = "Cafeteria Vernier";
       public static string[,] ERROR_MESSAGES = new string[,] 
       {
           {"Cafeteria Vernier","Fields Are Empty.","Wrong password.","Are you sure you want to logout ?","Error ! While converting","Image size must be less than 800 KB","Update successfully !","Error ! while update","Please select a option","Team name is not available","Team Added Successfully","You may have no team.","Member is already exits."},
           //       00                  01                  02                  03                          04                          05                              06                      07                      08                      09                          010                      011                        012
           {"Password not match","Weak","Medium","Strong","Invalid Email Address","Invalid Phone Number","Invalid IP Address","","","","","",""}
           //   10                11      12        13          14                      15                      16
       };
    }
}
