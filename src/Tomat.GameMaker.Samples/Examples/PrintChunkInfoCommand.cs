using System;
using System.IO;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.Samples.Examples;

[Command("print-chunk-info")]
public sealed class PrintChunkInfoCommand : BaseCommand {
    public override ValueTask ExecuteAsync(IConsole console) {
        ValidateWadExists();

        using var stream = File.Open(WadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var wad = GameMakerIffFile.FromStream(stream);
        if (wad.Form is null)
            throw new Exception("FORM chunk is null!");

        var form = wad.Form;
        if (form.Chunks is null)
            throw new Exception("FORM chunk's sub-chunks are null!");

        console.Output.WriteLine($"Chunk \"{form.Name}\" ({form.GetType().FullName})");
        console.Output.WriteLine($"Size: {form.Size} bytes");

        foreach (var chunk in form.Chunks.Values)
            PrintChunk(console, chunk);

        return default;
    }

    private static void PrintChunk(IConsole console, IGameMakerChunk chunk) {
        console.Output.WriteLine($"    Chunk \"{chunk.Name}\" ({chunk.GetType().FullName})");
        console.Output.WriteLine($"    Size: {chunk.Size} bytes");
    }
}
