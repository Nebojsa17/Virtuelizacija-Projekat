using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtuelizacija_Projekat.Common
{
    public class Logger
    {
        private string path;

        public Logger(string path) 
        {
            ValidatePath(path);
            this.path = path;
        }

        private void ValidatePath(string path) 
        {
            if (path.Length == 0) return;

            string directories = Path.GetDirectoryName(path);

            if (!Directory.Exists(directories)) Directory.CreateDirectory(directories);
            if (!File.Exists(path))
            {
                FileStream ms = File.Create(path);
                ms.Close();
                ms.Dispose();
            }
        }

        public void Log(string message) 
        {
            if (path.Length == 0) return;

            using (StreamWriter sw = new StreamWriter(path, append: true))
            {
                sw.WriteLine($"Logged: {DateTime.Now,-25} Message: {message}");
                sw.Close();
                sw.Dispose();
            }
        }
    }
}
