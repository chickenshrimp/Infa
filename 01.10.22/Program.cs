using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Infa
{
    public class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:5500/site/index.html/");
            listener.Start();
            Console.WriteLine("ожидание подключений...");
            var context = listener.GetContext();
            var request = context.Request;
            var response = context.Response;
            if(Directory.Exists(Path.GetFullPath("site")))
            {
                response.Headers.Set("Content-Type", "text/html");
                byte[] buffer;
                if(File.Exists(Path.GetFullPath("site/index.html")))
                {
                    buffer = File.ReadAllBytes("site/index.html");
                    response.ContentLength64 = buffer.Length;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                response.Close();
            }
            else
            {
                string responseString = "<html><head><meta charset='utf8'></head><body>ошибка 404 :((</body></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            
            listener.Stop();
            Console.WriteLine("обработка подключений завершена");
        }
    }
}
