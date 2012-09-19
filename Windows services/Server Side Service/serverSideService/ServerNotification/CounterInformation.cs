using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServerNotification
{
    [DataContract]
    public class CounterInformation
    {
        [DataMember]
        public string CounterName { get; set; }

        [DataMember]
        public string CounterNumber { get; set; }

        [DataMember]
        public string CustomerID { get; set; }

        [DataMember]
        public CounterStatus Status { get; set; }

        [DataMember]
        public DateTime sendingTime { get; set; }
    }

    [DataContract]
    public enum CounterStatus
    {
        [EnumMember]
        Free,
        [EnumMember]
        Busy
    }
}
