using BenchmarkDotNet.Running;

namespace Tomat.GameMaker.Benchmarks;

internal static class Program {
    internal static void Main(string[] args) {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
