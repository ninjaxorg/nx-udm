using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ninjax.UdmLib;

#pragma warning disable 1998

namespace nx_udm_cli
{
    class ServerCommand : Command
    {
        public ServerCommand() : base("server", "UDM Server")
        {
            Handler = CommandHandler.Create<IConsole, CancellationToken>(Run);
        }

        private async Task<int> Run(IConsole console, CancellationToken cancel)
        {
            var address = IPAddress.Parse("239.0.0.1");
            var endPoint = new IPEndPoint(address, 8088);
            using var server = new UdmServer(endPoint);

            console.Out.WriteLine($"S:JOINED: {endPoint}");

            for
            (
                var i = 0;
                i < 100 && !cancel.IsCancellationRequested;
                i++
            )
            {
                var message = $"{DateTime.Now} - message #{i}";
                console.Out.WriteLine($"S:TX: {message}");
                var data = Encoding.ASCII.GetBytes(message);
                
                server.Send(data);
                
                await Task.Delay(1_000, cancel);
            }

            return 0;
        }
    }
}
