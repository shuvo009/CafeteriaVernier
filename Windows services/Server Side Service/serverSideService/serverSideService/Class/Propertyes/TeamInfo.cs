using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Procesta.serverSideService.Class.Propertyes
{
    [DataContract]
   public class TeamInfo
    {
        [DataMember]
        public string TeamD { get; set; }
        [DataMember]
        public byte[] TeamImage { get; set; }
        [DataMember]
        public string TeamAdmin { get; set; }
        [DataMember]
        public short TotalMember { get; set; }
        [DataMember]
        public DateTime EntryDate { get; set; }
    }
}
