using System.Threading.Tasks;
using CliFx;

namespace Tomat.GameMaker.Samples;

internal static class Program {
    internal static async Task<int> Main(string[] args) {
        return await new CliApplicationBuilder()
                     .AddCommandsFromThisAssembly()
                     .Build()
                     .RunAsync(args);
    }
}
