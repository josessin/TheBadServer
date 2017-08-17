using System;

namespace ServerTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start Server
            Console.WriteLine("Starting server");
            HTTPServer server = new HTTPServer(80);
            //Start Server
            server.Start();
        }
    }
}
