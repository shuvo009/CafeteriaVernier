using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Procesta.CvServer.EntityFramework;
namespace Procesta.CvServer
{
   public class CustomerUserName  :ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                string customerUserName = (string)value;
                if (String.IsNullOrWhiteSpace(customerUserName) || String.IsNullOrEmpty(customerUserName))
                    return new ValidationResult(false, "Customer username is no empty.");
                using (Cafeteria_Vernier_dbEntities CVDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    if (CVDatabase.CustomerInformations.FirstOrDefault(CusUName => CusUName.UserID.Equals(customerUserName)) == null)
                        return new ValidationResult(true, null);
                    return new ValidationResult(false, "Customer username is not available.");
                }
            }
            catch
            {
                return new ValidationResult(false, "Error occur during checking Customer username.");
            }
        }
    }
}
