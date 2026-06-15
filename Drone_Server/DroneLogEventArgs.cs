using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Server
{
    public enum Direction { LOW, HIGH }
    public class DroneLogEventArgs : EventArgs
    {
        public string LogMessage { get; }
        public int Max { get; }
        public int Recieved { get; }
        public Direction Direction { get; }
        public DroneLogEventArgs(string eventMessage = "", int max = 0, int recieved = 0, Direction direction = Direction.LOW)
        {
            LogMessage = eventMessage;

            Recieved = recieved;
            Max = max;

            Direction = direction;
        }
    }
}
