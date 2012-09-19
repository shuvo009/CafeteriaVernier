using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Procesta.CVClient.Class.Methords;

namespace Procesta.CVClient.Class.ValidationRuls
{
    public class TeamNameValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string teamName = (string)value;
            if (teamName==null || string.IsNullOrEmpty(teamName) || string.IsNullOrWhiteSpace(teamName))
            {
                return new ValidationResult(false, "Please enter a team name");
            }
            if (ServerConnection.serviceFromServer.TeamNameChecker(teamName))
            {
                return new ValidationResult(true, "");
            }
            else
            {
                return new ValidationResult(false, "Team Name already exist");
            }
        }
    }
}
