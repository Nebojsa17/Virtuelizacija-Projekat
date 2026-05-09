using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Virtuelizacija_Projekat.Common;

namespace Drone_Client
{
    internal class Program
    {
        private static FileMenagment files;

        static void Main(string[] args)
        {
            string path = ConfigurationManager.AppSettings["flightDirectory"];

            files = new FileMenagment(path);

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
                        CSVManagment csvManager = new CSVManagment(files.Files[0], Int32.Parse(ConfigurationManager.AppSettings["rowRead"]));

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
                    "\n1 - Svi dostupni fajlovi" +
                    "\n2 - Posajli file (indx)" +
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
