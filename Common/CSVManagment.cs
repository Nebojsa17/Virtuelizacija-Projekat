using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CSVManagment
    {
        string path;
        int maxRows;
        private Logger logger;

        public CSVManagment(string path, int rows, Logger log) 
        {
            this.path = path;
            this.maxRows = rows;
            this.logger = log;
        }

        public void Proccess(IDroneService proxy) 
        {
            int rows = 0;

            foreach (var line in File.ReadLines(path))
            {
                rows++;
                //preskacemo prvi row zato sto je tu zaglavlje lol
                if (rows <= 1) continue;
                if (rows > maxRows+1) 
                {
                    logger.Log($"owerflow of rows: {rows}, in file {path}");
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
                    ProgressEnum progress = proxy.PushSample(new Sample(row));
                    Console.WriteLine($"loaded row {rows-1} - {row.Time}: service state in: "+progress);
                }
                catch (FaultException<SampleError> ex)
                {
                    // validation error !!!!
                    logger.Log($"row {rows} in file {path} failed validation, error: {ex.Message} ");
                }
                catch
                {
                    // los formatiran red !!!!
                    logger.Log($"row {rows} in file {path} is not in valid format.");
                }
            }
        }
    }
}
