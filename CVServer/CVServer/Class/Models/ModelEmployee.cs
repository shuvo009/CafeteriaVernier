using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
namespace Procesta.CvServer
{
   public class ModelEmployee :INotifyPropertyChanged,IDataErrorInfo
    {
        public string Name
        {
            get { return this._Name; }
            set
            {
                this._Name = value;
                this.onPropertyChnage("Name");
            }
        }

        public decimal? PhoneNmber
        {
            get { return this._PhoneNmber; }
            set
            {
                this._PhoneNmber = value;
                this.onPropertyChnage("PhoneNmber");
            }
        }

        public string Password
        {
            get { return this._Password; }
            set
            {
                this._Password = value;
                this.onPropertyChnage("Password");
            }
        }

        public string FirstPassword
        {
            get { return this._FirstPassword; }
            set
            {
                if (this._FirstPassword != value)
                {
                    this._FirstPassword = value;
                    this.onPropertyChnage("FirstPassword");
                }
            }
        }

        public string Address
        {
            get { return this._Address; }
            set
            {
                this._Address = value;
                this.onPropertyChnage("Address");
            }
        }
        public byte[] UserImage
        {
            get { return this._UserImage; }
            set
            {
                if (this._UserImage != value)
                {
                    this._UserImage = value;
                    this.onPropertyChnage("UserImage");
                }
            }
        }

        public ObservableCollection<ModelEmployPermissions> Permissions
        {
            get { return this._Permissions; }
            set
            {
                this._Permissions = value;
                this.onPropertyChnage("Permissions");
            }
        }
        #region Private Variable
        private string _Name;
        private decimal? _PhoneNmber;
        private string _Address;
        private string _Password;
        private byte[] _UserImage;
        private string _FirstPassword;
        private ObservableCollection<ModelEmployPermissions> _Permissions;
        #endregion

        #region Property Chnage
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChnage(string propertyName)
        {
            if (this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Error Info
        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            { 
                string errorMessages=string.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrEmpty(this.Name))
                        {
                            errorMessages = String.Format(CvVariables.DEFULT_ERROR_FORMATE,"Employee Name");
                        }
                        break;
                    case "PhoneNmber":
                        if (this.PhoneNmber==null)
                        {
                            errorMessages = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Employee Phone");
                        }
                        break;
                    case "Password":
                        if (String.IsNullOrEmpty(this.Password))
                        {
                            errorMessages = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Password");
                        }
                        else if(!(this.Password.Equals(this.FirstPassword)))
                        {
                            errorMessages = "Password does not match.";
                        }
                        break;
                    case "Address" :
                        if (String.IsNullOrEmpty(this.Address))
                        {
                            errorMessages = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Employee Address");
                        }
                        break;
                    default :
                        errorMessages = CvVariables.DEFAULT_ERROR_MESSAGES;
                        break;
                }
                return errorMessages;
            }
        }
        #endregion
    }
}
