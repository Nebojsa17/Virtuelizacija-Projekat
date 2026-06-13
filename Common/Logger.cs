using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Common
{
    public class Logger : IDisposable
    {
        private string path;
        private bool disposed = false;
        private TextWriter textWriter;

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

            try
            {
                textWriter = File.AppendText(path);
                
                textWriter.WriteLine($"Logged: {DateTime.Now,-25} Message: {message}");
                textWriter.Close();
                textWriter.Dispose();
                
            }
            catch 
            {
                if (textWriter != null)
                {
                    textWriter.Close();
                    textWriter.Dispose();
                }
            }
        }

        ~Logger()
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
                Console.WriteLine("Disposing LOGGER object!!!");
                if (disposing)
                {
                    if (textWriter != null)
                    {
                        textWriter.Dispose();
                        textWriter = null;
                    }
                }
                disposed = true;
            }
        }
    }
}
