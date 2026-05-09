using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Virtuelizacija_Projekat.Common
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

        public void Obradi() 
        {
            int rows = 0;

            foreach (var line in File.ReadLines(path))
            {
                rows++;
                if (rows <= 1) continue;
                if (rows > maxRows) break;

                if (string.IsNullOrWhiteSpace(line))
                {   
                    logger.Log($"row {rows,-4} in file {path,-20} is not in valid format");
                    continue;
                }
                try 
                {
                    DronInfo row = new DronInfo(line);
                    Console.WriteLine($"loaded row {rows-1} - {row.Time}");
                }
                catch (Exception ex)
                {
                    //los formatiran red !!!!
                    logger.Log($"row {rows,-4} in file {path,-20} is not valid. Recieved error: {ex}");
                }
            }
        }
    }
}
