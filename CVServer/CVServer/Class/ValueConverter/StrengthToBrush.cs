using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Procesta.CvServer.Class;
using System.Windows.Media;
namespace Procesta.CvServer.Class.ValueConverter
{
    public class StrengthToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value!=null)
            {
                if (value.ToString().Equals("Weak"))
                {
                    return new SolidColorBrush(Color.FromRgb(0xCC, 0x66, 0x00));
                }
                else if (value.ToString().Equals("Medium"))
                {
                    return new SolidColorBrush(Color.FromRgb(0xCC, 0xFF, 0x00));
                }
                else if (value.ToString().Equals("Strong"))
                {
                    return new SolidColorBrush(Color.FromRgb(0x00, 0xCC, 0x00));
                }
                else
                {
                    return new SolidColorBrush(Color.FromRgb(0x99, 0x00, 0x00));
                }
            }
            else
            {
                return new SolidColorBrush(Color.FromRgb(0x99, 0x00, 0xCC));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
