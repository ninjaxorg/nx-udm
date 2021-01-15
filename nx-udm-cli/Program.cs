using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace nx_udm_cli
{
    static class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand();
            rootCommand.AddCommand(new ClientCommand());
            rootCommand.AddCommand(new ServerCommand());

            try
            {
                return await rootCommand.InvokeAsync(args);
            }
            catch (OperationCanceledException)
            {
                return 1;
            }
        }
    }
}