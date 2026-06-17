using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class CSVManagment: IDisposable
    {
        string path;
        int maxRows;
        private bool disposed = false;
        private Logger logger;



        public CSVManagment(string path, int rows, string logPath) 
        {
            this.path = path;
            this.maxRows = rows;
            this.logger = new Logger(logPath);
        }

        public CSVManagment(string logPath)
        {
            this.logger = new Logger(logPath);
        }

        public void Proccess(IDroneService proxy) 
        {
            int rows = 0;
            ProgressEnum progress = ProgressEnum.IN_PROGRESS;
            using (StreamReader reader = new StreamReader(path))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    rows++;
                    //preskacemo prvi row zato sto je tu zaglavlje lol
                    if (rows <= 1) continue;
                    if (rows > maxRows + 1 || progress == ProgressEnum.COMPLETED)
                    {
                        logger.Log($"overflow of rows: {rows}, in file {path}");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        logger.Log($"row {rows} in file {path} is not in valid format.");
                        continue;
                    }

                    try
                    {
                        DronInfo row = new DronInfo(line);
                        progress = proxy.PushSample(new Sample(row));
                        Console.WriteLine($"loaded row {rows - 1,-4}\t service state: " + progress);
                        Thread.Sleep(60);
                    }
                    catch (FaultException<SampleError> ex)
                    {
                        // validation error !!!!
                        logger.Log($"row {rows} in file {path} failed validation, error: {ex.Detail.Message} at column {ex.Detail.Column} ");
                    }
                    catch
                    {
                        // los formatiran red !!!!
                        logger.Log($"row {rows} in file {path} is not in valid format.");
                    }
                }
            }
        }

        ~CSVManagment()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Console.WriteLine("Disposing CSVMANAGMENT object!!!");

                if (disposing)
                {
                    if (logger != null)
                    {
                        logger.Dispose();
                        logger = null;
                    }
                }
                disposed = true;
            }
        }
    }
}
