using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;

namespace ControlWork
{
    class WebServer
    {
        public static string fileOutput = @"C:\Users\okmay\source\repos\ControlWork\responseIndex.html";
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public static string SendResponse(HttpListenerRequest request)
        {
            return string.Format(File.ReadAllText(@"C:\Users\okmay\source\repos\ControlWork\index.html"), DateTime.Now);
        }

        public static void Main()
        {
            
            var ws = new WebServer(SendResponse, "http://localhost:8080/test/");
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
           
            string url = @"http://localhost:8080/test/";
            var awaiter = CallUrl(url);
            if (awaiter.Result != null)
            {
                File.WriteAllText(fileOutput, awaiter.Result);
                Console.WriteLine("HTML response output to" + fileOutput);
            }
            ws.resultMyVoid();
            ws.Stop();
        }

        public WebServer(IReadOnlyCollection<string> prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            }

           
            if (prefixes == null || prefixes.Count == 0)
            {
                throw new ArgumentException("URI prefixes are required");
            }

            if (method == null)
            {
                throw new ArgumentException("responder method required");
            }

            foreach (var s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
           : this(prefixes, method)
        {
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if (ctx == null)
                                {
                                    return;
                                }

                                var rstr = _responderMethod(ctx.Request);
                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            
                            finally
                            {
                                if (ctx != null)
                                {
                                    ctx.Response.OutputStream.Close();
                                }
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    
        public static async Task<string> CallUrl(string url)
        {
            HttpClient client = new HttpClient();
            var response  = client.GetStringAsync(url);
            return await response;
        }

        public void resultMyVoid()
        {
            var dict = new Dictionary<string, string>()
            {
                {"А", "A"},
                {"Б", "B"},
                {"Ц", "C" },
                {"Д", "D" },
                {"Е", "E"},
                {"Ф", "F" },
                {"Г", "G"},
                {"Ч", "H" },
                {"И", "I"},
                {"Ж", "J"},
                {"К", "K" },
                {"Л", "L" },
                {"М", "M" },
                {"Н", "N" },
                {"О", "O" },
                {"П", "P" },
                {"Й", "Q" },
                {"Р", "R"},
                {"С", "S" },
                {"Т", "T" },
                {"У", "U" },
                {"В", "V" },
                {"Ш", "W" },
                {"Х", "X"},
                {"Ю", "Y" },
                {"З", "Z"},
                {"а", "A"},
                {"б", "B"},
                {"ц", "C" },
                {"д", "D" },
                {"е", "E"},
                {"ф", "F" },
                {"г", "G"},
                {"ч", "H" },
                {"и", "I"},
                {"ж", "J"},
                {"к", "K" },
                {"л", "L" },
                {"м", "M" },
                {"н", "N" },
                {"о", "O" },
                {"п", "P" },
                {"й", "Q" },
                {"р", "R"},
                {"с", "S" },
                {"т", "T" },
                {"у", "U" },
                {"в", "V" },
                {"ш", "W" },
                {"х", "X"},
                {"ю", "Y" },
                {"з", "Z"}
            };
            StringBuilder header = new StringBuilder();
            File.WriteAllText(fileOutput, File.ReadAllText(@"C:\Users\okmay\source\repos\ControlWork\index.html"));
            string str = File.ReadAllText(fileOutput);

            str = str.Replace("@#!elephant=&.ha-ha", "resultMyVoid");
            File.WriteAllText(fileOutput, str);

            foreach (KeyValuePair<string, string> replacement in dict)
            {
                str = File.ReadAllText(fileOutput).Replace(replacement.Key, replacement.Value);
                File.WriteAllText(fileOutput, str);
            }
        }
    }
}

