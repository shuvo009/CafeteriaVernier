using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Procesta.CVClient.ServerSideService;
using System.Security.Permissions;
using System.Windows.Controls;
using Procesta.CVClient.ClientNotifaction;
using Procesta.CVClient.ServerNotifaction;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;
namespace Procesta.CVClient.Class.Methords
{
   public class ServerConnection
    {
       public static ServerSideServicesClient serviceFromServer = null;
       public static ClientNotificationClient clientNotifaction = null;
       public static ServerNotificationClient serverNotifaction = null;
       /// <summary>
       /// Connect To Server Services
       /// </summary>
       /// <returns></returns>
       public static bool ConnectToService()
       {
           try
           {
               serviceFromServer = new ServerSideServicesClient("NetTcpBinding_IServerSideServices");
               return true;
           }
           catch
           {
               return false;
           }
       }
       /// <summary>
       /// Connect and Send Information  to Server
       /// </summary>
       public static void NotifactionFromServer()
       {
           try
           {
               clientNotifaction = new ClientNotificationClient("NetTcpBinding_IClientNotification");
               string counterNumber=Properties.Settings.Default.CounterNumber.ToString();
               new Task(
                   new Action(() => 
                                {
                                    while (true)
                                    {
                                        try
                                        {
                                            ObservableCollection<CommandData> commands = new ObservableCollection<CommandData>(clientNotifaction.GetCommands(counterNumber));
                                            if (commands.Count > 0)
                                            {
                                                foreach (CommandData comm in commands)
                                                {
                                                    clientNotifaction.RemoveCommand(counterNumber);
                                                    switch (comm.Command)
                                                    {
                                                        case Commands.Shutdown:
                                                            Process.Start("shutdown", "/s /t 0");
                                                            break;
                                                        case Commands.Restart:
                                                            Process.Start("shutdown", "/r /t 0");
                                                            break;
                                                        case Commands.AccountLogout:
                                                           Counter.counterWindow.Dispatcher.BeginInvoke(new Action(Counter.counterWindow.Close), DispatcherPriority.Normal);
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            System.Threading.Thread.Sleep(6000);
                                        }
                                        catch
                                        {
                                            break;
                                        }
                                    }
                                })).Start();
           }
           catch
           {
               System.Threading.Thread.Sleep(10000);
               NotifactionFromServer();
           }
       }

       public static void NotifactionToServer()
       {
           try
           {
               serverNotifaction = new ServerNotificationClient("NetTcpBinding_IServerNotification");
               MiraculousMethods.conterInformation.CounterName = Properties.Settings.Default.CounterName;
               MiraculousMethods.conterInformation.CounterNumber = Properties.Settings.Default.CounterNumber.ToString();
               MiraculousMethods.conterInformation.CustomerID = null;
               MiraculousMethods.conterInformation.Status = CounterStatus.Free;
               new Task(new Action(() =>
                                       {
                                           while (true)
                                           {
                                               try
                                               {
                                                   MiraculousMethods.conterInformation.sendingTime = DateTime.Now;
                                                   serverNotifaction.SetCounterInformation(MiraculousMethods.conterInformation);
                                                   System.Threading.Thread.Sleep(5000);
                                               }
                                               catch 
                                               {
                                                   break;
                                               }
                                           }

                                       })).Start();
           }
           catch
           {
               System.Threading.Thread.Sleep(10000);
               NotifactionToServer();
           }
       }
       /// <summary>
       /// Check A Server Alive or Not
       /// </summary>
       /// <param name="serviceChannel"></param>
       /// <returns></returns>
       public static bool ServerAliveIs(ICommunicationObject serviceChannel)
       {
           if (serviceChannel.State.Equals(CommunicationState.Closed))
           {
               return false;
           }
           else if(serviceChannel.State.Equals(CommunicationState.Closing))
           {
               return false;
           }
           else if (serviceChannel.State.Equals(CommunicationState.Faulted))
           {
               return false;
           }
           else
           {
               return true;
           }
       }
    }
}
