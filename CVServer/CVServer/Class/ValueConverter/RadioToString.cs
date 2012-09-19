﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Procesta.CvServer
{
   public class RadioToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                string checkValue = value.ToString();
                string targetValue = parameter.ToString();
                return checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Binding.DoNothing;
            bool useValue = (bool)value;
            if (useValue)
            {
                return parameter;
            }
            return Binding.DoNothing;
        }
    }
}
