using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Procesta.ClientSideService.Class.Data;
using Procesta.ClientSideService.Class.Property;
using Procesta.RegeditClasss;
namespace Procesta.ClientSideService.Class.Methods
{
    public class LoadConfigration
    {

        public LoadConfigration()
       {
       }

        public List<ConfigerStatus> LoadCounterConfiger()
        {
            List<ConfigerStatus> AllCounterConfigration = new List<ConfigerStatus>();
            RegeditWriteRead registrCheck=new RegeditWriteRead();
            foreach (ResistryProperty configInfo in new RegistryData().registryDataCollection)
            {
                AllCounterConfigration.Add(new ConfigerStatus { Name = configInfo.Name, Status = registrCheck.CheckRegistryKey(configInfo.FullPath, configInfo.keyName) });
            }
            return AllCounterConfigration;
        }
    }
}
