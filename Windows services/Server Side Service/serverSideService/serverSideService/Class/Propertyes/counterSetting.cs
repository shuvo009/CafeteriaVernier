using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace Procesta.serverSideService.Class.Propertyes
{
    [DataContract]
    public class counterSetting
    {
        [DataMember]
       public int seconds { get; set; }
    }
}
