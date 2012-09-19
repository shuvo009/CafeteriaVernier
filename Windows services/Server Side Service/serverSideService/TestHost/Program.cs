using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ClientNotification;
using Procesta.serverSideService;
namespace TestHost
{
    class Program
    {
        public static void Main(string[] args)
        {

            new Program().testClientNotification();
        }

        private void testClientNotification()
        {
            ServiceHost myService = new ServiceHost(typeof(ServerSideServices));
            try
            {
                myService.Open();
                Console.ReadLine();
                Console.ReadLine();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
