using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Procesta.CvServer.EntityFramework;
namespace Procesta.CvServer
{
    public class TeamName :ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                string teamName = (string)value;
                if (String.IsNullOrWhiteSpace(teamName) || String.IsNullOrEmpty(teamName))
                    return new ValidationResult(false, "Team name is no empty.");
                using (Cafeteria_Vernier_dbEntities CVDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    if (CVDatabase.Teams.FirstOrDefault(tName => tName.Name.Equals(teamName)) == null)
                        return new ValidationResult(true, null);
                    return new ValidationResult(false, "Team name is not available.");
                }
            }
            catch
            {
                return new ValidationResult(false, "Error occur during checking team name.");
            }
        }
    }
}
