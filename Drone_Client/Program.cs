using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Common;

namespace Drone_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string downloadPath = ConfigurationManager.AppSettings["flightDirectory"];

            foreach(string file in FileMenagment.GetFilesInDirectory(downloadPath)) 
            {

            }

            Console.ReadKey();
        }
    }
}
