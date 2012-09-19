using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Procesta.CVClient.Class.CvCPropertes
{
   public class UserInfoViewer :INotifyPropertyChanged
    {
       private Int64 _Minutes;
       private byte[] _Photo;
       private string _Username;
       private string _Password;
       private string _TeamName = string.Empty;
       private Int64 _LoginHistoryID;
       public Int64 Minutes
       {
           get { return this._Minutes; }
           set { this._Minutes = value; this.OnPropertyChange("Minutes"); }
       }

       public byte[] Photo
       {
           get { return this._Photo; }
           set { this._Photo = value; this.OnPropertyChange("Photo"); }
       }

       public string Username
       {
           get { return this._Username; }
           set { this._Username = value; this.OnPropertyChange("Username"); }
       }

       public string Password
       {
           get { return this._Password; }
           set { this._Password = value; this.OnPropertyChange("Password"); }
       }

       public string TeamName
       {
           get { return this._TeamName; }
           set { this._TeamName = value; this.OnPropertyChange("TeamName"); }
       }

       public Int64 LoginHistoryID
       {
           get { return this._LoginHistoryID; }
           set { this._LoginHistoryID = value; this.OnPropertyChange("LoginHistoryID"); }
 
       }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged!=null)
            {
                this.PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
