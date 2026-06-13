using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Server
{
    public class DroneFunctions
    {
        public DroneFunctions() { }

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
        public void OnAccelerationSpike(object sender, EventArgs e)
        {
            Console.WriteLine("The drone is moving way to fast!!!");
        }
        public void OnOutOfBandWarning(object sender, EventArgs e)
        {
            Console.WriteLine("Spead ot of mean!!!");
        }
        public void OnWindSpike(object sender, EventArgs e)
        {
            Console.WriteLine("Wind is blowing hard!!!");
        }
    }
}
