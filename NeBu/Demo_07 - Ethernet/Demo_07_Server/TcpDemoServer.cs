// (C) 2021 Michael Kollegger
// 
// Kontakt mike@fotec.at / www.fotec.at
// 
// Erstversion vom 05.03.2021 08:17
// Entwickler      Michael Kollegger
// Projekt         NeBu

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo_07_Server
{
    /// <summary>
    /// <para>Demo TCP Server</para>
    /// Klasse TcpDemoServer. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class TcpDemoServer:IDisposable
    {
        readonly TcpListener _server = null;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _clientCounter = 0;

        public TcpDemoServer(IPAddress ip, int port)
        {
            _server = new TcpListener(ip, port);
            _server.Start();
            StartListener();
        }
        

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    _clientCounter++;
                    Console.WriteLine($"Client {_clientCounter} Connected!");

                    Task.Run(() =>
                    {
                        NetworkStream stream = client.GetStream();
                        String data = null;
                        Byte[] bytes = new Byte[256];
                        int i;
                        var clientId = _clientCounter;

                        try
                        {
                            // Loop to receive all the data sent by the client.
                            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0 || _cts.IsCancellationRequested)
                            {
                                if (_cts.IsCancellationRequested)
                                    break;

                                // Translate data bytes to a ASCII string.
                                data = Encoding.ASCII.GetString(bytes, 0, i);
                                Console.WriteLine($"Client {clientId} Message: {data}");

                                // Process the data sent by the client.
                                //data = data.ToUpper();

                                //byte[] msg = Encoding.ASCII.GetBytes(data);

                                //// Send back a response.
                                //stream.Write(msg, 0, msg.Length);
                                //Console.WriteLine("Sent: {0}", data);
                            }
                        }
                        catch
                        {
                            ;
                        }

                        stream.Close();
                        // Shutdown and end connection
                        Console.WriteLine($"Client {_clientCounter} Disconnected!");
                    });


                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                _server.Stop();
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}