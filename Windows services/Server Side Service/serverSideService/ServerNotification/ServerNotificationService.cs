using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections.ObjectModel;
namespace ServerNotification
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ServerNotificationService :IServerNotification
    {
        ObservableCollection<CounterInformation> ovcCounterInfo = new ObservableCollection<CounterInformation>();

        public void SetCounterInformation(CounterInformation counterInfo)
        {
            try
            {
                if (ovcCounterInfo.SingleOrDefault(x => x.CounterNumber == counterInfo.CounterNumber) == null)
                {
                    ovcCounterInfo.Add(counterInfo);
                }
            }
            catch
            {
                throw new FaultException("Unable to register");
            }
        }

        public ObservableCollection<CounterInformation> GetCounterInformation()
        {
            return ovcCounterInfo;
        }


        public void RemoveCounterInformation(CounterInformation counterInfo)
        {
            try
            {
                this.ovcCounterInfo.Remove(ovcCounterInfo.FirstOrDefault(x => x.CounterNumber.Equals(counterInfo.CounterNumber)));
            }
            catch (Exception ex) 
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
