using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Drone_Server
{
    public class DroneService : IDroneService
    {
        private static int recievedSamplesCnt = 0;
        private static bool inSession = false;
        public static int MaxRead;

        public ConfirmationEnum EndSession()
        {
            if(!inSession) return ConfirmationEnum.NACK;

            Console.WriteLine("Session ended!!!");
            inSession = false;
            return ConfirmationEnum.ACK;
        }

        public ProgressEnum PushSample(Sample sample)
        {

            if (recievedSamplesCnt >= MaxRead) return ProgressEnum.COMPLETED;

            recievedSamplesCnt++;

            try
            {
                Validate(sample);
            }
            catch (FaultException<SampleError> er) 
            {
                Console.WriteLine($"recieved sample [{recievedSamplesCnt}/{MaxRead}]\t has error: {er.Detail.Message} at column {er.Detail.Column}");
                throw er;
            }

            Console.WriteLine($"recieved sample [{recievedSamplesCnt}/{MaxRead}]");
            
            if(recievedSamplesCnt >= MaxRead)
            {
                return ProgressEnum.COMPLETED;
            }
            else 
            {
                return ProgressEnum.IN_PROGRESS;
            }
        }

        public ConfirmationEnum StartSession(MetaHeader meta)
        {
            if (inSession) return ConfirmationEnum.NACK;

            Console.WriteLine("Session started!!!");
            recievedSamplesCnt = 0;
            inSession = true;
            return ConfirmationEnum.ACK;
        }

        private void Validate(Sample sample) 
        {
            if (sample == null)
                throw new FaultException<SampleError>(new SampleError { Message = "Sample is null", Column = "Sample" },
                                                      new FaultReason("Sample validation failed"));
            if (sample.WindSpeed < 0)
                throw new FaultException<SampleError>(new SampleError { Message = "Wind speed shouldn't be negative", Column = "WindSpeed" },
                                                      new FaultReason("Sample validation failed"));
            if (sample.WindAngle < 0 || sample.WindAngle > 359)
                throw new FaultException<SampleError>(new SampleError { Message = "Wind angle must be between 0 and 359", Column = "WindAngle" },
                                                      new FaultReason("Sample validation failed"));
            if (sample.Time < 0)
                throw new FaultException<SampleError>(new SampleError { Message = "Time cant be negative", Column = "Time" },
                                                      new FaultReason("Sample validation failed"));
        }
    }
}
