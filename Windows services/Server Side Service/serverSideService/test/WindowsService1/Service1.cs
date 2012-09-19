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
namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        private  ServiceHost serverSideHost = null;
        protected override void OnStart(string[] args)
        {
         //    MiraculousMethod.ConnectionStringChanger();
            serverSideHost = new ServiceHost(typeof(ServerSideServices));
            serverSideHost.AddServiceEndpoint(typeof(IServerSideServices), new NetTcpBinding(), string.Format("net.tcp://192.168.1:9079/ServerSideServices"));
            serverSideHost.Open();

            //serverSideHost = new ServiceHost(typeof(ServerSideServices));
            //serverSideHost.Open();
        }

        protected override void OnStop()
        {
        }
    }
}
