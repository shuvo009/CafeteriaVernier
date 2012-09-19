using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel;
namespace Cv.CvClientServerSide
{
    [DataContract]
    public class ConterConfigratons : INotifyPropertyChanged
    {
        private string _Name;
        private bool _IsChecked;

        [DataMember]
        public string Name
        {
            get { return this._Name; }
            set
            {
                this._Name = value;
                this.OnPropertyChange("Name");
            }
        }

        [DataMember]
        public bool IsChecked
        {
            get { return this._IsChecked; }
            set
            {
                this._IsChecked = value;
                this.OnPropertyChange("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged!=null)
            {
               this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
