using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Procesta.CvServer.Class.ValueConverter
{
    public class CounterStatusEnToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                //CommonUse.CounterStatues.CounterStatusEn conterStatus = (CommonUse.CounterStatues.CounterStatusEn)value;
                //if (conterStatus.Equals(CommonUse.CounterStatues.CounterStatusEn.Free))
                //{
                //    return "/Images/CounterFree.png";
                //}
                //else if (conterStatus.Equals(CommonUse.CounterStatues.CounterStatusEn.Login))
                //{
                //    return "/Images/CopunterBusy.png";
                //}
                //else if (conterStatus.Equals(CommonUse.CounterStatues.CounterStatusEn.Lock))
                //{
                //     return "/Images/CounterLock.png";
                //}
                //else
                //{
                return "/Images/CounterError.png";
                //}
            }
            catch
            {
                 return "/Images/CounterError.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
