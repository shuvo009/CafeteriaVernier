using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Text.RegularExpressions;
namespace Procesta.CvServer
{
   public class EmailAddressValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value!=null)
            {
                Regex emailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
                if (emailRegex.Match(value.ToString()).Success)
                {
                    return new ValidationResult(true, string.Empty);
                }
                else
                {
                    return new ValidationResult(false, "Please enter a valid E-Mail address.");
                }
            }
            else
            {
                return new ValidationResult(true, string.Empty);
            }
        }
    }
}
