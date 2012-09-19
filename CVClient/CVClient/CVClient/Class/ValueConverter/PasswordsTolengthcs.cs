using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Procesta.CVClient.Class.ValueConverter
{
    public class PasswordsTolengthcs : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((string)values[0]!=string.Empty && (string)values[1]!=string.Empty && ((string)values[0]).Equals((string)values[1]) && ((string)values[0]).Length.Equals(((string)values[1]).Length))
            {
                if (((string)values[1]).Length <= 6)
                {
                    return 1.0;
                }
                else if (((string)values[1]).Length <= 11)
                {
                    return 2.0;
                }
                else
                {
                    return 3.0;
                }
            }
            else
            {
                return 0.0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            object[] obj = new object[1];
            return obj;
        }
    }
}
