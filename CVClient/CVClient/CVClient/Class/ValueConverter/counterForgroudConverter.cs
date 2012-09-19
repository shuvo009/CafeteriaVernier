using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Procesta.CVClient.Class.ValueConverter
{
    public class counterForgroudConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            LinearGradientBrush counterForgroundBrush = new LinearGradientBrush();
            counterForgroundBrush.StartPoint = new System.Windows.Point(0.5, 0);
            counterForgroundBrush.EndPoint = new System.Windows.Point(0.5, 1);
            if (value != null)
            {
                int munities=(int)value;
                if (munities>=20)
                {
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF299503"), 0));
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF94EB75"), 1));
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF05AF17"), 0.5));
                    return counterForgroundBrush;
                }
                else if(munities>=10)
                {
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFD5D600"), 0));
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFEBDC75"), 1));
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFB5B600"), 0.5));
                    return counterForgroundBrush;
                }
                else
                {
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFD01010"), 0));
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFE44343"), 1));
                    counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFB60000"), 0.5));
                    return counterForgroundBrush;
                }
            }
            else
            {
                counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF299503"), 0));
                counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF94EB75"), 1));
                counterForgroundBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF05AF17"), 0.5));
                return counterForgroundBrush;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
