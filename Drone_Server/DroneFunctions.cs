using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Server
{
    public class DroneFunctions
    {
        Logger log;
        public DroneFunctions() 
        {
            log = new Logger("Logs/drone_flight_spikes.txt");
        }

        public void OnStartSession(object sender, EventArgs e)
        {
            Console.WriteLine("New Session started!!!");
        }

        public void OnEndSession(object sender, EventArgs e)
        {
            Console.WriteLine("Session finished!!!");
        }

        public void OnRecieveSample(object sender, DroneLogEventArgs e)
        {
            Console.WriteLine($"Recieved sample [{e.Recieved}/{e.Max}]");
        }

        public void OnErrorSample(object sender, DroneLogEventArgs e)
        {
            Console.WriteLine(e.LogMessage);
        }
        public void OnAccelerationSpike(object sender, DroneLogEventArgs e)
        {
            switch (e.Direction) 
            {
                case Direction.LOW:
                    Console.WriteLine("The drone suddenly slowed down!");
                    log.Log("The drone suddenly slowed down! At sample: " + e.Recieved);
                    break;
                case Direction.HIGH:
                    Console.WriteLine("The drone suddenly accelerated!");
                    log.Log("The drone suddenly accelerated! At sample: " + e.Recieved);
                    break;
                default:
                    Console.WriteLine("The drone's movements are too sudden!");
                    log.Log("The drone's movements are too sudden! At sample: " + e.Recieved);
                    break;
            }
        }
        public void OnOutOfBandWarning(object sender, DroneLogEventArgs e)
        {
            switch (e.Direction)
            {
                case Direction.LOW:
                    Console.WriteLine("Low diverging speed!");
                    log.Log("Low diverging speed! At sample: " + e.Recieved);
                    break;
                case Direction.HIGH:
                    Console.WriteLine("High diverging speed!");
                    log.Log("High diverging speed! At sample: " + e.Recieved);
                    break;
                default:
                    Console.WriteLine("Diverging speed!");
                    log.Log("Diverging speed! At sample: " + e.Recieved);
                    break;
            }
        }
        public void OnWindSpike(object sender, DroneLogEventArgs e)
        {
            switch (e.Direction)
            {
                case Direction.LOW:
                    Console.WriteLine("Slow wind!");
                    log.Log("Slow wind! At sample: " + e.Recieved);
                    break;
                case Direction.HIGH:
                    Console.WriteLine("High wind!");
                    log.Log("High wind! At sample: " + e.Recieved);
                    break;
                default:
                    Console.WriteLine("Wind!");
                    log.Log("Wind! At sample: " + e.Recieved);
                    break;
            }
        }
    }
}
