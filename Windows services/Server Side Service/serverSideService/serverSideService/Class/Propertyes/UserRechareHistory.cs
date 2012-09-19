using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Procesta.serverSideService.Class.Propertyes
{
   [DataContract]
   public class UserRechareHistory
    {
       [DataMember]
       public string  EmployID { get; set; }
       [DataMember]
       public DateTime ? EntryDate { get; set; }
       [DataMember]
       public int ? Munities { get; set; }
       [DataMember]
       public decimal ? Amount { get; set; }
    }
}
