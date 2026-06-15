using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Server
{
    public class DroneService : IDroneService
    {
        private static int recievedSamplesCnt = 0;
        private static bool inSession = false;
        public static int MaxRead;

        public static double Amean;
        public static double Aprev = 0;

        public static double Anorm;
        public static double dA;
        public static double Weffect;

        public static DroneEventPublisher droneEvents;

        public ConfirmationEnum EndSession()
        {
            if(!inSession) return ConfirmationEnum.NACK;

            droneEvents.EndTransfer();
            inSession = false;
            return ConfirmationEnum.ACK;
        }

        public ProgressEnum PushSample(Sample sample)
        {
            var reportsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ReportsDirectory"]);
            var measurementsPath = Path.Combine(reportsPath, ConfigurationManager.AppSettings["MeasurementsFile"]);
            var rejectsPath = Path.Combine(reportsPath, ConfigurationManager.AppSettings["RejectsFile"]);

            if (recievedSamplesCnt >= MaxRead) return ProgressEnum.COMPLETED;

            recievedSamplesCnt++;

            droneEvents.Recieved(recievedSamplesCnt, MaxRead);

            try
            {
                Validate(sample);
            }
            catch (FaultException<SampleError> er) 
            {
                droneEvents.Warning($"sample error [{recievedSamplesCnt}/{MaxRead}]\t has error: {er.Detail.Message} at column {er.Detail.Column}");
                throw;
            }

            bool invalid = Analytics(sample);

            if (invalid)
            {
                if (File.Exists(rejectsPath))
                {
                    using (StreamWriter sw = new StreamWriter(rejectsPath, true))
                    {
                        sw.WriteLine($"{sample.LinearAccelerationX},{sample.LinearAccelerationY},{sample.LinearAccelerationZ},{sample.WindSpeed},{sample.WindAngle},{sample.Time},{Anorm},{dA},{Amean},{Weffect}");
                    }
                }
            }
            else if (File.Exists(measurementsPath))
            {
                using (StreamWriter sw = new StreamWriter(measurementsPath, true))
                {
                    sw.WriteLine($"{sample.LinearAccelerationX},{sample.LinearAccelerationY},{sample.LinearAccelerationZ},{sample.WindSpeed},{sample.WindAngle},{sample.Time},{Anorm},{dA},{Amean},{Weffect}");
                }
            }


            if (recievedSamplesCnt >= MaxRead)
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

            droneEvents.StartTransfer();

            recievedSamplesCnt = 0;
            inSession = true;

            var reportsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ReportsDirectory"]);
            var measurementsPath = Path.Combine(reportsPath, ConfigurationManager.AppSettings["MeasurementsFile"]);
            var rejectsPath = Path.Combine(reportsPath, ConfigurationManager.AppSettings["RejectsFile"]);

            if (!Directory.Exists(reportsPath))
            {
                Directory.CreateDirectory(reportsPath);
            }

            if (!File.Exists(measurementsPath))
            {
                using (StreamWriter sw = File.CreateText(measurementsPath))
                {
                    sw.WriteLine(string.Join(",", meta.Header) + ",Anorm,dA,Amean,Weffect");
                }
            }

            if (!File.Exists(rejectsPath))
            {
                using (StreamWriter sw = File.CreateText(rejectsPath))
                {
                    sw.WriteLine(string.Join(",", meta.Header) + ",Anorm,dA,Amean,Weffect");
                }
            }

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

        private bool Analytics(Sample sample)
        {
            Anorm = Math.Sqrt(Math.Pow(sample.LinearAccelerationX, 2) + Math.Pow(sample.LinearAccelerationY, 2) + Math.Pow(sample.LinearAccelerationZ, 2));
            Amean = (Amean * (recievedSamplesCnt - 1) + Anorm) / recievedSamplesCnt;
            Weffect = Math.Abs(sample.WindSpeed * Math.Sin(sample.WindAngle));

            dA = Anorm - Aprev;
            Aprev = Anorm;

            double Athreshold = double.Parse(ConfigurationManager.AppSettings["A_threshold"]);
            double Wthreshold = double.Parse(ConfigurationManager.AppSettings["W_threshold"]);
            double Deviation = double.Parse(ConfigurationManager.AppSettings["Deviation"]);

            bool invalid = false;

            if (dA > Athreshold)
            {
                droneEvents.AccelerationSpike(Direction.HIGH);
                invalid = true;
            }
            else if (dA < -Athreshold)
            {
                droneEvents.AccelerationSpike(Direction.LOW);
                invalid = true;
            }

            if (Weffect > Wthreshold)
            {
                droneEvents.WindSpike(Direction.HIGH);
                invalid = true;
            }
            else if (Weffect < -Wthreshold)
            {
                droneEvents.WindSpike(Direction.LOW);
                invalid = true;
            }

            if (Anorm < (1 - Deviation) * Amean)
            {
                droneEvents.OutOfBandWarning(Direction.LOW);
                invalid = true;
            }
            else if (Anorm > (1 + Deviation) * Amean)
            {
                droneEvents.OutOfBandWarning(Direction.HIGH);
                invalid = true;
            }

                return invalid;
        }
    }
}
