using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Procesta.CvServer.Class.ValueConverter
{
    public class MunitiesToColor : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value!=null)
            {
                Int32 munities =System.Convert.ToInt32(value);
                if (munities <= 5)
                {
                    return new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x33));
                }
                else if (munities<=10)
                {
                    return new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    return new SolidColorBrush(Colors.Wheat);
                }
            }
            else
            {
                return new SolidColorBrush(Colors.Wheat);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
