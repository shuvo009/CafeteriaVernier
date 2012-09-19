using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Procesta.CvServer
{
    class ModelCustomer: INotifyPropertyChanged,IDataErrorInfo
    {

        public string UserName
        {
            get { return this._UserName; }
            set
            {
                if (this._UserName != value)
                {
                    this._UserName = value;
                    this.onPropertyChanged("UserName");
                }
            }
        }

        public string Password
        {
            get { return this._Password; }
            set
            {
                if (this._Password != value)
                {
                    this._Password = value;
                    this.onPropertyChanged("Password");
                }
            }
        }

        public string CheckPassword
        {
            get { return this._CheckPassword; }
            set
            {
                if (this._CheckPassword != value)
                {
                    this._CheckPassword = value;
                    this.onPropertyChanged("CheckPassword");
                }
            }
        }

        public string Name
        {
            get { return this._Name; }
            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.onPropertyChanged("Name");
                }
            }
        }

        public string Phone
        {
            get { return this._Phone; }
            set
            {
                if (this._Phone != value)
                {
                    this._Phone = value;
                    this.onPropertyChanged("Phone");
                }
            }
        }

        public string Email
        {
            get { return this._Email; }
            set
            {
                if (this._Email != value)
                {
                    this._Email = value;
                    this.onPropertyChanged("Email");
                }
            }
        }

        public string NationalID
        {
            get { return this._NationalID; }
            set
            {
                if (this._NationalID != value)
                {
                    this._NationalID = value;
                    this.onPropertyChanged("NationalID");
                }
            }
        }

        public DateTime JoinDate
        {
            get { return this._JoinDate; }
            set
            {
                if (this._JoinDate != value)
                {
                    this._JoinDate = value;
                    this.onPropertyChanged("JoinDate");
                }
            }
        }

        public string Address
        {
            get { return this._Address; }
            set
            {
                if (this._Address != value)
                {
                    this._Address = value;
                    this.onPropertyChanged("Address");
                }
            }
        }

        public byte[] Image
        {
            get { return this._Image; }
            set
            {
                if (this._Image != value)
                {
                    this._Image = value;
                    this.onPropertyChanged("Image");
                }
            }
        }

        public int Minutes
        {
            get { return this._Minutes; }
            set
            {
                if (this._Minutes != value)
                {
                    this._Minutes = value;
                    this.onPropertyChanged("Minutes");
                }
            }
        }

        #region Private Variable
        private string _UserName;
        private string _Password;
        private string _CheckPassword;
        private string _Name;
        private string _Phone;
        private string _Email;
        private string _NationalID;
        private DateTime _JoinDate;
        private string _Address;
        private byte[] _Image;
        private int _Minutes;
        #endregion

        #region Property Change
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged!=null)
            {
               this.PropertyChanged(this,new PropertyChangedEventArgs(propertyName));                
            }
        }
        #endregion

        #region Data Error
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
                    case "UserName":
                        if (String.IsNullOrEmpty(this.UserName))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "UserName");
                        }
                        break;
                    case "Password":
                        if (String.IsNullOrEmpty(this.Password))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Password");
                        }
                        else if( !(this.Password.Equals(this.CheckPassword)))
                        {
                            errorMessagess = "Password does not match.";
                        }
                        break;
                    case "Name":
                        if (String.IsNullOrEmpty(this.UserName))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Name");
                        }
                        break;
                    case "Phone":
                        if (String.IsNullOrEmpty(this.UserName))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Phone");
                        }
                        break;
                    default:
                        break;
                }
                return errorMessagess;
            }
        }
        #endregion
    }
}
