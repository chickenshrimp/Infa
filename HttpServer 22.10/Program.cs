using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace HttpsSteam
{
    internal class Program
    {
        private static bool appIsRunning = true;
        static void Main()
        {
            using(var server = new HttpServer())
            {
                server.Start();
                while (appIsRunning)
                {
                    Handler(Console.ReadLine()?.ToLower(), server);
                }
            }
        }

        static void Handler(string command, HttpServer server)
        {
            switch (command)
            {
                case "stop":
                    server.Stop();
                    break;
                case "start":
                    server.Start();
                    break;
                case "restart":
                    server.Stop();
                    server.Start();
                    break;
                case "status": 
                    Console.WriteLine(server.Status.ToString());
                    break;
                case "exit": 
                    appIsRunning = false;
                    break;
            }
        }
    }
}
