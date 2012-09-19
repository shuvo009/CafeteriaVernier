using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
namespace Procesta.CvServer
{
    class ModelTeamInfo :INotifyPropertyChanged,IDataErrorInfo
    {
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

        public string AdminName
        {
            get { return this._AdminName; }
            set
            {
                if (this._AdminName != value)
                {
                    this._AdminName = value;
                    this.onPropertyChanged("AdminName");
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

        public ObservableCollection<ModelCommonUse> TeamMemberList
        {
            get { return this._TeamMemberList; }
            set
            {
                if (this._TeamMemberList != value)
                {
                    this._TeamMemberList = value;
                    this.onPropertyChanged("TeamMemberList");
                }
            }
        }

        public int Minutes
        {
            get { return this._Minutes; }
            set 
            {
                this._Minutes = value;
                this.onPropertyChanged("Minutes");
            }
        }

        #region Private Variables
        private string _Name;
        private byte[] _Image;
        private string _AdminName;
        private DateTime _JoinDate;
        private ObservableCollection<ModelCommonUse> _TeamMemberList;
        private int _Minutes;
        #endregion

        #region On Property Change
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion

        #region Data Error Info
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get 
            {
                string errorMessagess = string.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrEmpty(this.Name))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Team Name");
                        }
                        break;
                    case "AdminName":
                        if (String.IsNullOrEmpty(this.AdminName))
                        {
                            errorMessagess = String.Format(CvVariables.DEFULT_ERROR_FORMATE, "Admin name");
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
