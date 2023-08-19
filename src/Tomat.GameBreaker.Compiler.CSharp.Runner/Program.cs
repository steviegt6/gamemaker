using System.IO;
using Tomat.GameMaker.IFF;

namespace Tomat.GameBreaker.Compiler.CSharp.Runner;

internal static class Program {
    internal static void Main(string[] args) {
        _ = GameMakerIffFile.FromStream(File.OpenRead(args[0]), out var ctx);
        var compiler = new CodeCompilation(ctx);
        var assembly = compiler.Compile();
        assembly.Write(Path.GetFileNameWithoutExtension(args[0]) + ".dll");
    }
}
