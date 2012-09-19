using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Procesta.CvServer
{
    public class ModelBillConfig :INotifyPropertyChanged,IDataErrorInfo
    {
        public string Minutes 
        {
            get { return this._Minutes; }
            set
            {
                if (this._Minutes!=value)
                {
                    this._Minutes = value; OnPropertyChange("Minutes");
                }
            }
        }
        public string Amount 
        {
            get { return this._Amount; }
            set
            {
                if (this._Amount != value)
                {
                    this._Amount = value;
                    OnPropertyChange("Amount");
                }
            }
        }

        #region Private Variables
        private string _Minutes;
        private string _Amount;
        #endregion

        #region onProperty Chnage
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string PropertyName)
        {
            if (PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get 
            {
                string errorMessagess = string.Empty;
                switch (columnName)
                {
                    case "Amount":
                        if (String.IsNullOrEmpty(this.Amount))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Rate");
                        }
                        break;
                    case "Munities":
                        if (String.IsNullOrEmpty(this.Minutes))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Minutes");
                        }
                        break;
                    default:
                        break;
                }
                return errorMessagess;
            }
        }
    }
}
