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

        public CSVManagment(string path, int rows) 
        {
            this.path = path;
            this.maxRows = rows;
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
                    //empty line counts as bad
                    continue;

                try 
                {
                    DronInfo row = new DronInfo(line);
                    Console.WriteLine($"line {rows-1} - {row.Time}");
                }
                catch (Exception ex)
                {
                    //los formatiran red !!!!
                    Console.WriteLine("Ne ispravan red :(\nGreska: "+ex.Message);
                }


                //Console.WriteLine($"\t{line}");
            }
        }
    }
}
