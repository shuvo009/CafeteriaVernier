using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections.ObjectModel;
namespace ClientNotification
{
    [ServiceContract]
    public interface IClientNotification
    {
        [OperationContract(IsOneWay=true)]
        void setCommand(CommandData command);
        [OperationContract]
        ObservableCollection<CommandData> GetCommands(string counterNumber);
        [OperationContract]
        void RemoveCommand(string counterNumber);
    }
}
