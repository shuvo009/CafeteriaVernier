using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Procesta.CvServer.EntityFramework;
namespace Procesta.CvServer
{
   public class EmployeeName : ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                string employeeName = (string)value;
                if (String.IsNullOrWhiteSpace(employeeName) || String.IsNullOrEmpty(employeeName))
                    return new ValidationResult(false, "Employee name is no empty.");
                using (Cafeteria_Vernier_dbEntities CVDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    if (CVDatabase.Employees.FirstOrDefault(empName => empName.EmployeeID.Equals(employeeName)) == null)
                        return new ValidationResult(true, null);
                    return new ValidationResult(false, "Employee name is not available.");
                }
            }
            catch 
            {
                return new ValidationResult(false, "Error occur during checking employee name.");
            }
        }
    }
}
