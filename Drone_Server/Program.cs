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
            DroneEventPublisher publisher = new DroneEventPublisher();
            DroneService.droneEvents = publisher;
            DroneFunctions droneFunctions = new DroneFunctions();

            publisher.OnTransferStarted += droneFunctions.OnStartSession;
            publisher.OnTransferCompleted += droneFunctions.OnEndSession;
            publisher.OnSampleReceived += droneFunctions.OnRecieveSample;
            publisher.OnWarningRaised += droneFunctions.OnErrorSample;
            publisher.OnAccelerationSpike += droneFunctions.OnAccelerationSpike;
            publisher.OnOutOfBandWarning += droneFunctions.OnOutOfBandWarning;
            publisher.OnWindSpike += droneFunctions.OnWindSpike;

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
