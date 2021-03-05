﻿// (C) 2020 Michael Kollegger
// 
// Kontakt mike@fotec.at / www.fotec.at
// 
// Erstversion vom 04.03.2020 20:27
// Entwickler      Michael Kollegger
// Projekt         NeBu

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using Demo_07_Server;

namespace Demo_07_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                args = new[] {"127.0.0.1"};
            }

            var client = new TcpDemoClient(args[0]);
            Console.WriteLine("Connected to server!");
            Console.WriteLine("Type quit to end applikation!");
            do
            {
                Console.Write("Message: ");
                var msg = Console.ReadLine();
                if (string.Compare(msg, "quit", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    break;
                }
                client.Send(msg);
            } while (true);
        }




        static void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}