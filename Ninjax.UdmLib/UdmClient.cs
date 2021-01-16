using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ninjax.UdmLib
{
    public class UdmClient : IDisposable
    {
        private readonly UdpClient _udp;
        private bool _alive = true;

        public UdmClient(IPEndPoint endpoint, Action<byte[], IPEndPoint> receiver)
        {
            _udp = new UdpClient {ExclusiveAddressUse = false};
            _udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            
            _udp.Client.Bind(new IPEndPoint(IPAddress.Any, endpoint.Port));
            
            _udp.JoinMulticastGroup(endpoint.Address);
            
            Task.Run(async delegate
            {
                while (_alive)
                {
                    var result = await _udp.ReceiveAsync();
                    receiver(result.Buffer, result.RemoteEndPoint);
                }
            });
        }

        public void Dispose()
        {
            _alive = false;
            _udp.Dispose();
        }
    }
}