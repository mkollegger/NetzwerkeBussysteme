using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Demo_07_Server
{
    class MyTcpListener
    {
        public static void Main()
        {
            Task.Run(() =>
            {
                var s = new TcpDemoServer(IPAddress.Any, 13000);
            });

            Console.WriteLine("Server started!");
            Console.WriteLine("Hit Enter to shutdown ...");
            Console.ReadLine();
        }
    }
}