using System;
using System.Net;
using System.Net.Sockets;

namespace Ninjax.UdmLib
{
    public class UdmServer : IDisposable
    {
        private readonly IPEndPoint _endpoint;
        private readonly UdpClient _udp;

        public UdmServer(IPEndPoint endpoint)
        {
            _endpoint = endpoint;
            _udp = new UdpClient();
            _udp.JoinMulticastGroup(endpoint.Address);
        }

        public void Send(byte[] data)
        {
            _udp.Send(data, data.Length, _endpoint);
        }

        public void Dispose()
        {
            _udp.Dispose();
        }
    }
}
