using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ninjax.UdmLib;

#pragma warning disable 1998

namespace nx_udm_cli
{
    class ClientCommand:Command
    {
        public ClientCommand() : base("client", "UDM Client")
        {
            AddOption(new Option<int>("--port", () => 8088, "port"));
            AddOption(new Option<string>("--host", () => "239.0.0.1", "host"));
            Handler = CommandHandler.Create<ClientOptions, IConsole, CancellationToken>(Run);
        }

        private async Task<int> Run(ClientOptions options, IConsole console, CancellationToken cancel)
        {
            if (options.Host == null)
            {
                console.Error.WriteLine($"Host not specified");
                return 1;
            }

            var address = IPAddress.Parse(options.Host);
            var endPoint = new IPEndPoint(address, options.Port);

            console.Out.WriteLine($"C:JOINED: {endPoint}");

            using var client = new UdmClient(endPoint, (data, ep) =>
            {
                var message = Encoding.ASCII.GetString(data);
                console.Out.WriteLine($"C:RX: [{ep}] - {message}");
            });

            cancel.WaitHandle.WaitOne();

            return 0;
        }

        public class ClientOptions
        {
            public int Port { get; set; }
            public string? Host { get; set; }
        }
    }
}