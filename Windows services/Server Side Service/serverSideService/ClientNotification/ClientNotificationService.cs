using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections.ObjectModel;
namespace ClientNotification
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, IncludeExceptionDetailInFaults=true)]
    public class ClientNotificationService :IClientNotification
    {
        ObservableCollection<CommandData> ovcCommands = new ObservableCollection<CommandData>();
        public void setCommand(CommandData command)
        {
            try
            {
                ovcCommands.Add(command);   
            }
            catch
            {
                throw new FaultException("Unable to send command");
            }
        }

        public ObservableCollection<CommandData> GetCommands(string counterNumber)
        {
            return new ObservableCollection<CommandData>(ovcCommands.Where(x => x.CounterNumber == counterNumber));   
        }

        public void RemoveCommand(string counterNumber)
        {
            try
            {
                this.ovcCommands.Remove(this.ovcCommands.First(x => x.CounterNumber == counterNumber));
            }
            catch
            {
 
            }
        }


     
    }
}
