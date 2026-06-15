using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Server
{
    public class DroneLogEventArgs : EventArgs
    {
        public string LogMessage { get; }
        public int Max { get; }
        public int Recieved { get; }
        public DroneLogEventArgs(string eventMessage)
        {
            LogMessage = eventMessage;
        }
        public DroneLogEventArgs(int max, int recieved)
        {
            Recieved = recieved;
            Max = max;
        }
        public DroneLogEventArgs(string eventMessage, int max, int recieved)
        {
            LogMessage = eventMessage;

            Recieved = recieved;
            Max = max;
        }
    }
}
