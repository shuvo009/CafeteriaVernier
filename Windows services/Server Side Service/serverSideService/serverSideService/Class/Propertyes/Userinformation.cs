using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Procesta.serverSideService.Class.Propertyes
{
    [DataContract]
   public class Userinformation
    {
        [DataMember]
        public byte[] UserImage { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string NationalID { get; set; }
        [DataMember]
        public DateTime ? Date { get; set; }
        [DataMember]
        public string Address { get; set; }
    }
}
