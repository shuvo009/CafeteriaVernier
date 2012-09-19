using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Procesta.CvServer.Class;

namespace Procesta.CvServer.Class.ValueConverter
{
    public class MenuParmissionToImage :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value!=null)
            {
                string item = value.ToString().Trim();
                if(item.Equals(CvVariables.MENU_PERMISSION[0]))
                {
                    return "/ButtonImages/NewCustomer.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[1]))
                {
                    return "/ButtonImages/EditCustomer.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[2]))
                {
                    return "/ButtonImages/Recharge Account.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[3]))
                {
                    return "/ButtonImages/EditRecharges.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[4]))
                {
                   return "/ButtonImages/RechargeHistory.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[5]))
                {
                   return "/ButtonImages/UserLoginHistory.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[6]))
                {
                   return "/ButtonImages/CashView.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[7]))
                {
                   return "/ButtonImages/CashEdit.png";//
                }
                else if(item.Equals(CvVariables.MENU_PERMISSION[8]))
                {
                   return "/ButtonImages/TodaySummary.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[9]))
                {
                    return "/ButtonImages/settings.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[10]))
                {
                    return "/ButtonImages/MoneySetting.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[11]))
                {
                     return "/ButtonImages/userMaintenance.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[12]))
                {
                    return "/ButtonImages/TeamMaintenance.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[13]))
                {
                    return "/ButtonImages/AddTeam.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[14]))
                {
                    return "/ButtonImages/ChangePassword.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[15]))
                {
                    return "/ButtonImages/ScreenCaptureSetting.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[16]))
                {
                    return "/ButtonImages/SenEmail.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[17]))
                {
                    return "/ButtonImages/EmailSetting.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[18]))
                {
                    return "/ButtonImages/General.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[19]))
                {
                    return "/ButtonImages/Home.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[20]))
                {
                    return "/ButtonImages/counterSetting.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[21]))
                {
                    return "/ButtonImages/ConterConfiger.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[22]))
                {
                    return "/ButtonImages/AddEmploy.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[23]))
                {
                    return "/ButtonImages/EditEmploy.png";
                }
                else if (item.Equals(CvVariables.MENU_PERMISSION[24]))
                {
                    return "/ButtonImages/database.png";
                }  
                else if (item.Equals(CvVariables.MENU_PERMISSION[25]))
                {
                    return "/ButtonImages/UserReset.png";
                }
                else
                {
                   return "";
                }
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
