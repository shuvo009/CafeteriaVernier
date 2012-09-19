using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Procesta.serverSideService;
using Procesta.serverSideService.Class;
using Procesta.serverSideService.Class.LinqToSql;
using Procesta.serverSideService.Class.Propertyes;
using Procesta.serverSideService.Class.Methods;
using System.ServiceModel.Description;
using System.Net;
using System.Configuration;
namespace ConsoleApplication1
{
    class Program : ConfigurationSection 
    {
        private static ServiceHost serverSideHost = null;
        static void Main(string[] args)
        {
            //MiraculousMethod.ConnectionStringChanger();
            serverSideHost = new ServiceHost(typeof(ServerSideServices));
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.Security.Mode = SecurityMode.None;
            serverSideHost.AddServiceEndpoint(typeof(IServerSideServices), tcp, string.Format("net.tcp://{0}:9079/ServerSideServices","192.168.1.1"));
            serverSideHost.Open();

            //var dd = ConfigurationManager.GetSection("configuration");
            //Console.WriteLine("Key: {0}, Value: ", dd.ToString());

            //foreach (var key in ConfigurationManager.AppSettings)
            //{

            //    //string value = ConfigurationManager.ConnectionStrings[key];

            //    Console.WriteLine("Key: {0}, Value: ", key.ToString());

            //}
            Console.Read();
        }



        private static string HostIpChecker()
        {
            IPAddress[] localPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress WorkingIp in localPs)
            {
                if (WorkingIp.ToString().StartsWith("192."))
                {
                    return WorkingIp.ToString();
                }
            }
            return string.Empty;
        }     
    }
}
