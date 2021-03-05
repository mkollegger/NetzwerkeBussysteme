// (C) 2021 Michael Kollegger
// 
// Kontakt mike@fotec.at / www.fotec.at
// 
// Erstversion vom 05.03.2021 08:48
// Entwickler      Michael Kollegger
// Projekt         NeBu

using System;
using System.Net.Sockets;
using System.Text;

namespace Demo_07_Server
{
    /// <summary>
    /// <para>Demo Client</para>
    /// Klasse TcpDemoClient. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class TcpDemoClient:IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private bool _connected;

        public TcpDemoClient(string ip)
        {
            Connect(ip);
        }

        private void Connect(string ip)
        {
            var port = 13000;

            try
            {
                _client = new TcpClient(ip, port);
                _stream = _client.GetStream();
                _connected = true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public void Send(string msg)
        {
            if (!_connected)
                return;
            var data = Encoding.ASCII.GetBytes(msg);
            _stream.Write(data);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _client?.Dispose();
            _stream?.Dispose();
        }
    }
}