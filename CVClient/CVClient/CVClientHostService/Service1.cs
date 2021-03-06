﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace CVClientHostService
{
    public partial class Service1 : ServiceBase
    {
        private ServiceHost serverSideHost;
        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "ServiceOnClient";
            this.CanStop = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\error.txt", true))
            {
                file.WriteLine(HostPropertys.HostIpAddress);
            }
            serverSideHost = new ServiceHost(typeof(ClientSideService));
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.Security.Mode = SecurityMode.None;
            serverSideHost.AddServiceEndpoint(typeof(IClientSideService), tcp, string.Format("net.tcp://{0}:9080/ClientSideService", HostPropertys.HostIpAddress));
            serverSideHost.Open();
        }

        protected override void OnStop()
        {
            serverSideHost.Close();
        }
    }
}
