using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
namespace Procesta.CvServer.Class.ValueConverter
{
    public class MenuToolTipFormate : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value!=null)
            {
                return string.Format(" {0} ", value.ToString().Trim());
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
