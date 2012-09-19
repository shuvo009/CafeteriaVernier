using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Procesta.CvServer.Class;
namespace Procesta.CvServer.Class.ValueConverter
{
    public class PasswordsToStrength : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value!=null && ((double)value)> 0.0)
            {
                if (((double)value).Equals(1.0))
                {
                    return "Weak";
                }
                else if (((double)value).Equals(2.0))
                {
                    return "Medium";
                }
                else
                {
                    return "Strong";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
