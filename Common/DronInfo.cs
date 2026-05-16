using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DronInfo
    {
        public double Time { get; set; }
        public double WindSpeed { get; set; }
        public double WindAngle { get; set; }
        public double BatteryVoltage { get; set; }
        public double BatteryCurrent { get; set; }

        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double PositionZ { get; set; }

        public double OrientationX { get; set; }
        public double OrientationY { get; set; }
        public double OrientationZ { get; set; }
        public double OrientationW { get; set; }

        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public double VelocityZ { get; set; }

        public double AngularX { get; set; }
        public double AngularY { get; set; }
        public double AngularZ { get; set; }

        public double LinearAccelerationX { get; set; }
        public double LinearAccelerationY { get; set; }
        public double LinearAccelerationZ { get; set; }

        public DronInfo(string line)
        {
            var culture = CultureInfo.InvariantCulture;

            var parts = line.Split(',');

            Time = double.Parse(parts[0], culture);
            WindSpeed = double.Parse(parts[1], culture);
            WindAngle = double.Parse(parts[2], culture);
            BatteryVoltage = double.Parse(parts[3], culture);
            BatteryCurrent = double.Parse(parts[4], culture);

            PositionX = double.Parse(parts[5], culture);
            PositionY = double.Parse(parts[6], culture);
            PositionZ = double.Parse(parts[7], culture);

            OrientationX = double.Parse(parts[8], culture);
            OrientationY = double.Parse(parts[9], culture);
            OrientationZ = double.Parse(parts[10], culture);
            OrientationW = double.Parse(parts[11], culture);

            VelocityX = double.Parse(parts[12], culture);
            VelocityY = double.Parse(parts[13], culture);
            VelocityZ = double.Parse(parts[14], culture);

            AngularX = double.Parse(parts[15], culture);
            AngularY = double.Parse(parts[16], culture);
            AngularZ = double.Parse(parts[17], culture);

            LinearAccelerationX = double.Parse(parts[18], culture);
            LinearAccelerationY = double.Parse(parts[19], culture);
            LinearAccelerationZ = double.Parse(parts[20], culture);
        }
    }
}
