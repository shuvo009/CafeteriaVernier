using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
namespace Procesta.CvServer
{
    public class DateDiffer :IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return (DateTime)values[0] - (DateTime)values[1];
            }
            catch
            {
                return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
