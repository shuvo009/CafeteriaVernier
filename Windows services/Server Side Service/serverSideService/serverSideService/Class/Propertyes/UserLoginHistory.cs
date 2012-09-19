using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace Procesta.serverSideService.Class.Propertyes
{
   [DataContract]
   public class LoginHistory
    {
       [DataMember]
       public DateTime ? EntryDate { get; set; }
       [DataMember]
       public int ? MinutesUse { get; set; }
       [DataMember]
       public short ? CounterNumber { get; set; }
       [DataMember]
       public string  TeamName { get; set; }
    }
}
