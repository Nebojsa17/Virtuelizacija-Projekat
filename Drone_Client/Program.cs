using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Common;
using System.ServiceModel;
using System.IO;

namespace Drone_Client
{
    internal class Program
    {
        //za rad sa datotekama
        private static FileMenagment files;

        //promenljive iz appconfig
        private static string flightsPath;
        private static string loggerPath;
        private static int maxRows;
        private static string[] meta;

        static void Main(string[] args)
        {
            Console.Write("Press any to start..... ");
            Console.ReadLine();

            //veza sa servisom
            ChannelFactory<IDroneService> factory = new ChannelFactory<IDroneService>("DroneService");
            IDroneService proxy = factory.CreateChannel();

            //ucitavanje iz appconfig
            flightsPath = ConfigurationManager.AppSettings["flightDirectory"];
            loggerPath = ConfigurationManager.AppSettings["logDirectory"];
            maxRows = Int32.Parse(ConfigurationManager.AppSettings["rowRead"]);
            meta = ConfigurationManager.AppSettings["meta"].Split(',');

            //objekti potrebni za rad
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
                        if (files.Files.Length > 0) FileProccessing(proxy);
                        else
                        {
                            Console.WriteLine("\nNo rows for processing");
                            Console.WriteLine("\nPress any to continue...");
                            Console.ReadKey();
                        }
                        break;
                }

            } while (menuRespone != 3);
        }

        public static void FileProccessing(IDroneService proxy)
        {
            int row = -1;

            do
            {
                Console.Write("Input file index: ");
                try
                {
                    row = int.Parse(Console.ReadLine());
                }
                catch
                {
                    row = -1;
                }
            } while (!(row >= 1 && row <= files.Files.Length));

            ConfirmationEnum conf = proxy.StartSession(new MetaHeader { Header = meta });

            switch (conf)
            {
                case ConfirmationEnum.ACK:
                    CSVManagment csvManager = new CSVManagment(files.Files[row - 1], maxRows, loggerPath);
                    
                    try
                    {
                        csvManager.Proccess(proxy);
                    }
                    catch (IOException)
                    {
                        Console.WriteLine($"neuspesno otvaranje fajla: {files.Files[row - 1]}");
                        csvManager.Dispose();
                    }

                    proxy.EndSession();
                    break;
                case ConfirmationEnum.NACK:
                    Console.WriteLine("\nService didn't acknowledge start of session :(");
                    break;
            }

            Console.WriteLine("\nAll done!!!!\nPress any to continue...");
            Console.ReadKey();
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
