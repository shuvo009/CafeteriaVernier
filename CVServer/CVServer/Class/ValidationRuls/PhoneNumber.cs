using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Text.RegularExpressions;
namespace Procesta.CvServer
{
   public class PhoneNumber : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                string phoneNumber = (string)value;
                Regex emailRegex = new Regex(@"\b01[5-9|1][0-9]{8,8}\b");
                if(emailRegex.IsMatch(phoneNumber))
                    return new ValidationResult(true, null);
                return new ValidationResult(false, "Please enter a valid phone number.");
            }
            catch
            {
                return new ValidationResult(false, "Error occur during checking phone number.");
            }
        }
    }
}
