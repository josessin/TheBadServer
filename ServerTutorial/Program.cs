using System;

namespace ServerTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server");
            HTTPServer server = new HTTPServer(80);
            server.Start();
        }
    }
}
