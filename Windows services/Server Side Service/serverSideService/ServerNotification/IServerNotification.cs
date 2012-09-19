using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections.ObjectModel;
namespace ServerNotification
{
    [ServiceContract]
    public interface IServerNotification
    {
        [OperationContract(IsOneWay = true)]
        void SetCounterInformation(CounterInformation counterInfo);
        [OperationContract]
        ObservableCollection<CounterInformation> GetCounterInformation();
        [OperationContract]
        void RemoveCounterInformation(CounterInformation counterInfo);

    }
}
