using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Procesta.CvServer.ServerNotifaction;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
namespace Procesta.CvServer.Class.Methods
{
    public class NotificationFromClients : INotifyPropertyChanged
    {
        private ServerNotificationClient serverNotify = null;

        private ObservableCollection<CounterInformation> _CounterInformations;
        public ObservableCollection<CounterInformation> CounterInformations
        {
            get
            {
                return this._CounterInformations;
            }
            set 
            {
                this._CounterInformations = value;
                if (this.PropertyChanged!=null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CounterInformations"));
                }
            }
        }
        public NotificationFromClients()
        {
            this.CounterInformations = new ObservableCollection<CounterInformation>();
            serverNotify = new ServerNotificationClient("NetNamedPipeBinding_IServerNotification");
            this.GetNotifactions();
        }

        private void GetNotifactions()
        {
            new Task(new Action(() =>
                                    {
                                        while (true)
                                        {
                                            try
                                            {
                                                this.CounterInformations = new ObservableCollection<CounterInformation>(this.serverNotify.GetCounterInformation());
                                                foreach (CounterInformation counterInfo in this.CounterInformations)
                                                {
                                                    var dd = DateTime.Now- counterInfo.sendingTime;
                                                    if ((DateTime.Now-counterInfo.sendingTime) > new TimeSpan(0, 0, 5))
                                                    {
                                                        this.serverNotify.RemoveCounterInformation(counterInfo);
                                                    }
                                                }
                                                System.Threading.Thread.Sleep(5000);
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    })).Start();
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
