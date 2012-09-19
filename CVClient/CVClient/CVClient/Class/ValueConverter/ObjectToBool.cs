using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Procesta.CVClient.Class.ValueConverter
{
    [ValueConversion(typeof(Object), typeof(bool))]
    public class ObjectToBool : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values[0].ToString().Equals(values[1].ToString().Trim());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
