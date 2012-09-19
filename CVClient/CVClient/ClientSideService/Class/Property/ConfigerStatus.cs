using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Procesta.ClientSideService.Class.Property
{
    [DataContract]
   public class ConfigerStatus
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Status { get; set; }
    }
}
