using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CommonUse;
using System.Security.Permissions;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                   DuplexChannelFactory<IServerWithCallback> cf =
                 new DuplexChannelFactory<IServerWithCallback>(
                     new CallbackImpl(),
                     new NetTcpBinding(),
                     new EndpointAddress("net.tcp://192.168.1.1:9078/DataService"));
                  IServerWithCallback srv = cf.CreateChannel();
                  CounterStatues cs = new CounterStatues { CounterName = "Counter eeee", CounterNumber = "ttt", CounterStatus =CounterStatues.CounterStatusEn.Lock, UserId = "yahoo", IpAddress = "192.168.50.90" };

                srv.StartDataOutput(cs);
                System.Threading.ParameterizedThreadStart pt = new System.Threading.ParameterizedThreadStart(ServerChecker);
                System.Threading.Thread checkserver = new System.Threading.Thread(pt);
                checkserver.Start(srv);
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try again ...");
                System.Threading.Thread.Sleep(600);
                Main(null);
            }
        }
        [HostProtectionAttribute(SecurityAction.LinkDemand, Synchronization = true,ExternalThreading = true)]

        public static void ServerChecker(object csd)
        {
            try
            {
                for (; ; )
                {
                    System.Threading.Thread.Sleep(500);
                    Console.WriteLine("I am Start");
                    ((IServerWithCallback)csd).ClientCalls();
                }
            }
            catch
            {
                Main(null);
            }
          
        }
    }
}
