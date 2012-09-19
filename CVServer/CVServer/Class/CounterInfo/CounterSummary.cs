using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Procesta.CvServer.Class.CounterInfo
{
    public class CounterSummary : INotifyPropertyChanged
    {
        private string _CounterName;
        private string _CounterIpAddress;

        public string CounterName
        {
            get { return this._CounterName; }
            set
            {
                this._CounterName = value;
                this.OnPropertyChanged("CounterName");
            }
        }

        public string CounterIpAddress
        {
            get { return this._CounterIpAddress; }
            set
            {
                this._CounterIpAddress = value;
                this.OnPropertyChanged("CounterIpAddress");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                // Raise the PropertyChanged event
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
