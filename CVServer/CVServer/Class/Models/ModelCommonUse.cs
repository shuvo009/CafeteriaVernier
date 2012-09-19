using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Procesta.CvServer 
{
   public class ModelCommonUse : INotifyPropertyChanged
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
  
        #region Private Property 
        private string _UserName;
        private byte[] _Image;
        #endregion

        #region onProperty Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
