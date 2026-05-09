using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Virtuelizacija_Projekat.Common
{
    public class FileMenagment
    {
        public string[] Files { get; set; }

        public FileMenagment(string directory) 
        {
            Files = GetFilesInDirectory(directory);
        }

        public string FileName(int index)
        {
            try
            {
                FileInfo fi = new FileInfo(Files[index]);
                
                return fi.Name;
            }
            catch  
            {
                return "";
            }
        }

        public void PrintAvailableFiles() 
        {
            Console.WriteLine("Available files: \n");

            for (int i = 0; i < Files.Length; i++)
            {
                if (i + 2 < Files.Length)
                {
                    Console.WriteLine($"\t{i + 1,3} - {FileName(i),-10}\t{i + 2,3} - {FileName(i + 1),-10}\t{i + 3,3} - {FileName(i + 2),-10}");
                    i += 2;
                    continue;
                }

                if (i + 1 < Files.Length)
                {
                    Console.WriteLine($"\t{i + 1,3} - {FileName(i),-10}\t{i + 2,3} - {FileName(i+1),-10}");
                    i += 1;
                    continue;
                }
                Console.WriteLine($"\t{i + 1,3} - {FileName(i),-10}");
            }
        }

        public static string[] GetFilesInDirectory(string directoryPath) 
        {
            string[] files = new string[0];

            if (!Directory.Exists(directoryPath)) 
            {
                Console.WriteLine($"\tPath ${directoryPath} isnt valid!");
                return files;
            }

            files = Directory.GetFiles(directoryPath);

            return files;
        }
    }
}
