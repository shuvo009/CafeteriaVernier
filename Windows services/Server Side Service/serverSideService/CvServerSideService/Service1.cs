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
using ClientNotification;
using ServerNotification;
namespace CvServerSideService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "CvServerServices";
            this.CanStop = true;
            this.AutoLog = true;
        }
        ServiceHost ClientNotification = new ServiceHost(typeof(ClientNotificationService));
        ServiceHost ServerNotifaction = new ServiceHost(typeof(ServerNotificationService));
        ServiceHost ServerService = new ServiceHost(typeof(ServerSideServices));
        protected override void OnStart(string[] args)
        {
            ClientNotification.Open();
            ServerNotifaction.Open();
            ServerService.Open();
        }

        protected override void OnStop()
        {
            if (ClientNotification.State == CommunicationState.Opened)
            {
                ClientNotification.Close();
            }
            if (ServerService.State == CommunicationState.Opened)
            {
                ServerService.Close();
            }
            if (ServerNotifaction.State == CommunicationState.Opened)
            {
                ServerNotifaction.Close();
            }
        }
    }
}
