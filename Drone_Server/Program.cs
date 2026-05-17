using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;

namespace Drone_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(DroneService));
            host.Open();

            DroneService.MaxRead = Int32.Parse(ConfigurationManager.AppSettings["MaxRead"]);

            Console.WriteLine("Service is open, press any key to close it.");
            Console.ReadKey();

            host.Close();
            Console.WriteLine("Service is closed");
        }
    }
}
