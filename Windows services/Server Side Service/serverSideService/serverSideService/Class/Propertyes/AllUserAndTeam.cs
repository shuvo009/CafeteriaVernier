using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Procesta.serverSideService.Class.Propertyes
{
    [DataContract]
   public class AllUserAndTeam
    {
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// This Property use only when a team Member Delete or Add
        /// True for add
        /// False for Remove
        /// </summary>
        [DataMember]
        public bool AddOrDelete { get; set; }

    }

}
