using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using Telerik.Windows.Controls;
namespace CommonUse
{
    /// <summary>
    /// This service Will be run at 9078 port
    /// </summary>
    [ServiceContract(Namespace = "rf.services",
       CallbackContract = typeof(IDataOutputCallback),
       SessionMode = SessionMode.Required)]
    public interface IServerWithCallback
    {
        [OperationContract(IsOneWay = true)]
        void StartDataOutput(CounterStatues counterStatuse);
        [OperationContract(IsOneWay = true)]
        void ClientCalls();
    }


    public interface IDataOutputCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendDataPacket();
    }

    [DataContract]
    public class CounterStatues :INotifyPropertyChanged
    {
        public enum CounterStatusEn{Free,Login,Lock}

        private string _CounterName;
        private string _CounterNumber;
        private string _IpAddress;
        private CounterStatusEn _CounterStatus;
        private string _UserId;
        private TileViewItemState _State;
        [DataMember]
        public string CounterName
        {
            get { return this._CounterName; }
            set
            {
                this._CounterName = value;
                this.OnPropertyChange("CounterName");
            }
        }

        [DataMember]
        public string CounterNumber
        {
            get { return this._CounterNumber; }
            set
            {
                this._CounterNumber = value;
                this.OnPropertyChange("CounterNumber");
            }
        }

        [DataMember]
        public string IpAddress
        {
            get { return this._IpAddress; }
            set
            {
                this._IpAddress = value;
                this.OnPropertyChange("IpAddress");
            }
        }

        [DataMember]
        public CounterStatusEn CounterStatus
        {
            get { return this._CounterStatus; }
            set
            {
                this._CounterStatus = value;
                this.OnPropertyChange("CounterStatus");
            }
        }

        [DataMember]
        public string UserId
        {
            get { return this._UserId; }
            set
            {
                this._UserId = value;
                this.OnPropertyChange("UserId");
            }
        }

        [DataMember]
        public TileViewItemState State
        {
            get { return this._State; }
            set
            {
                this._State = value;
                this.OnPropertyChange("State");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange(string PropertyName)
        {
            if (PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
