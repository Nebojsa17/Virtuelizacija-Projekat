using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Virtuelizacija_Projekat.Common;
using System.ServiceModel;

namespace Drone_Client
{
    internal class Program
    {
        private static FileMenagment files;
        private static Logger logger;

        static void Main(string[] args)
        {
            //veza sa servisom
            ChannelFactory<IDroneService> factory = new ChannelFactory<IDroneService>("DroneService");
            IDroneService proxy = factory.CreateChannel();

            //ucitavanje iz appconfig
            string flightsPath = ConfigurationManager.AppSettings["flightDirectory"];
            string loggerPath = ConfigurationManager.AppSettings["logDirectory"];
            int maxRows = Int32.Parse(ConfigurationManager.AppSettings["rowRead"]);

            //objekti potrebni za rad
            logger = new Logger(loggerPath);
            files = new FileMenagment(flightsPath);

            int menuRespone = 0;

            do 
            {
                menuRespone = Menu();

                switch (menuRespone) 
                {
                    case 1:
                        files.PrintAvailableFiles();
                        Console.WriteLine("\nPress any to continue...");
                        Console.ReadKey();
                        break;
                    case 2:
                        CSVManagment csvManager = new CSVManagment(files.Files[0], maxRows,logger);

                        csvManager.Obradi();

                        Console.WriteLine("\nPress any to continue...");
                        Console.ReadKey();
                        break;
                }

            }while (menuRespone != 3);
        }

        public static int Menu() 
        {
            int response = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\t\tDrone Delivery\n" +
                    "\n1 - All files" +
                    "\n2 - Send file for processing" +
                    "\n3 - Exit");
                try 
                {

                    response = Int32.Parse(Console.ReadKey(true).KeyChar.ToString());
                }
                catch 
                {
                    response = 0;
                }

            } while (!(response >= 1 && response <= 3));

            Console.Clear();
            return response;
        }
    }
}
