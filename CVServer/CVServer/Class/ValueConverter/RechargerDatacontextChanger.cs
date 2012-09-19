using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Procesta.CvServer
{
    /// <summary>
    /// value[0] : radioButtonUser
    /// value[1] : radioButtonTeam
    /// value[2] : AccountRecUser.SelectedItem
    /// value[3] : AccountRecTeam.SelectedItem
    /// </summary>
   public class RechargerDatacontextChanger : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values!=null)
            {
                try
                {
                    if ((bool)values[0])
                    {
                        return values[2];
                    }
                    else if((bool)values[1])
                    {
                        return values[3];
                    }
                    else
                    {
                        return Binding.DoNothing;
                    }
                }
                catch
                {
                    return Binding.DoNothing;
                }
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
