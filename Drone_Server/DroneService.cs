using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Server
{
    public class DroneService : IDroneService
    {
        public ConfirmationEnum EndSession()
        {
            Console.WriteLine("Session ended!!!");
            return ConfirmationEnum.ACK;
        }

        public ProgressEnum PushSample(Sample sample)
        {
            try
            {
                Validate(sample);
                Console.WriteLine($"recieved sample: {sample.Time}");
                return ProgressEnum.IN_PROGRESS;
            }
            catch (Exception ex)
            {
                throw new FaultException<SampleError>(  new SampleError { Message = ex.Message },
                                                        new FaultReason("Sample validation failed"));
            }
        }

        public ConfirmationEnum StartSession(MetaHeader meta)
        {
            Console.WriteLine("Session started!!!");
            return ConfirmationEnum.ACK;
        }

        private void Validate(Sample sample) 
        {
            if (sample == null)
                throw new Exception("Sample is null");
            if (sample.WindSpeed < 0)
                throw new Exception("Wind speed shouldn't be negative");
            if (sample.WindAngle < 0 || sample.WindAngle > 359)
                throw new Exception("Wind angle must be between 0 and 359");
            if (sample.Time < 0)
                throw new Exception("Time cant be negative");
        }
    }
}
