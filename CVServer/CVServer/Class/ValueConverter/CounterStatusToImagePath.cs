using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Procesta.CvServer.ServerNotifaction;

namespace Procesta.CvServer
{
    public class CounterStatusToImagePath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { 
            if (value !=null)
            {
                CounterStatus conStatus = (CounterStatus)value;
                if (conStatus.Equals(CounterStatus.Free))
                {
                    return "/Images/CounterFree.png";
                }
                else
                {
                    return "/Images/CopunterBusy.png";
                }

            }
            else
            {
                return "/Images/CounterFree.png";;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
