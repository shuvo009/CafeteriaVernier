using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Procesta.CvServer.Class.ValueConverter
{
    [ValueConversion(typeof(string),typeof(double))]
    public class PasswordsTolengthcs : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0]!=null && values[1]!=null && values[0].ToString()!=string.Empty && values[1].ToString()!=string.Empty)
            {
                if (values[0].ToString().Equals(values[1].ToString()))
                {
                    if (values[1].ToString().Length <= 6)
                    {
                        return 1.0;
                    }
                    else if (values[1].ToString().Length <= 11)
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
