using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using Procesta.serverSideService;
using Procesta.serverSideService.Class;
using Procesta.serverSideService.Class.LinqToSql;
using Procesta.serverSideService.Class.Propertyes;
using Procesta.serverSideService.Class.Methords;
using System.ServiceModel.Description;
using System.Net;
namespace HotServrSideService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        //private ServiceHost serverSideHost = null;
        protected override void OnStart(string[] args)
        {
            //if (serverSideHost!=null)
            //{
            //    serverSideHost.Close();
            //}
            //new MiraculousMethod().ConnectionStringChanger();
            //serverSideHost = new ServiceHost(typeof(ServerSideServices));
            //serverSideHost.AddServiceEndpoint(typeof(IServerSideServices), new NetTcpBinding(), string.Format("net.tcp://{0}:9079/ServerSideServices", this.HostIpChecker()));
            //serverSideHost.Open();
        }

        protected override void OnStop()
        {
            //if (serverSideHost!=null)
            //{
            //    serverSideHost.Close();
            //}
        }

        /// <summary>
        /// Collect Local Host IP
        /// </summary>
        //private string HostIpChecker()
        //{
        //       IPAddress[] localPs = Dns.GetHostAddresses(Dns.GetHostName());
        //        foreach (IPAddress WorkingIp in localPs)
        //        {
        //            if (WorkingIp.ToString().StartsWith("192."))
        //            {
        //                return WorkingIp.ToString();
        //            }
        //         }
        //     return string.Empty;
        //  }     
    }
}
