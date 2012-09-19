using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
namespace Procesta.CvServer.Class.ValueConverter
{
    public class IsLoginToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                return " Logout ";
            }
            else
            {
                return " Login ";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
