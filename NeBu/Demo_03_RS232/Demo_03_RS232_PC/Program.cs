using System;
using System.IO.Ports;

namespace Demo_03_RS232_PC
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            port.DataReceived += PortOnDataReceived;
            port.Open();

            Console.WriteLine("Any key to exit!");
            Console.ReadKey();
        }

        private static void PortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var p = sender as SerialPort;
            Console.Write(p.ReadExisting());
        }
    }
}
