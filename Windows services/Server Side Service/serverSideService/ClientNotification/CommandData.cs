using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ClientNotification
{
    [DataContract]
    public class CommandData
    {
        [DataMember]
        public string CounterNumber { get; set; }
        [DataMember]
        public Commands Command { get; set; }
    }

    [DataContract]
    public enum Commands
    {
        [EnumMember]
        Shutdown,
        [EnumMember]
        Restart,
        [EnumMember]
        AccountLogout
    }
}
