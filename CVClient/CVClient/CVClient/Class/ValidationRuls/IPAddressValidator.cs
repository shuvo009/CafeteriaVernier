using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Text.RegularExpressions;
namespace Procesta.CVClient.Class.ValidationRuls
{
    public class IPAddressValidator :ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value!=null)
            {
                Regex ipAddressRegex = new Regex(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
                if (ipAddressRegex.Match(value.ToString()).Success)
                {
                    return new ValidationResult(true, string.Empty);
                }
                else
                {
                    return new ValidationResult(false, "Please enter a valid IP Address");
                }
            }
            else
            {
                return new ValidationResult(true,string.Empty);
            }
        }
    }
}
