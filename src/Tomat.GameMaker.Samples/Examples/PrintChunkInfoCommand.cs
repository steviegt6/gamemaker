﻿using System;
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
        var wad = GameMakerIffFile.FromStream(stream, out _);
        var form = wad.Form;

        console.Output.WriteLine($"Chunk \"{form.Name}\" ({form.GetType().FullName})");
        console.Output.WriteLine($"Size: {form.Size} bytes");
        console.Output.WriteLine($"Sub-chunks: {form.Chunks.Count}");

        foreach (var chunk in form.Chunks.Values)
            PrintChunk(console, chunk);

        console.Output.WriteLine();
        console.Output.WriteLine("Unknown chunks:");

        foreach (var chunk in form.Chunks.Values) {
            if (chunk is GameMakerUnknownChunk unknownChunk)
                console.Output.WriteLine($"    {unknownChunk.Name} ({unknownChunk.GetType().FullName})");
        }

        return default;
    }

    private static void PrintChunk(IConsole console, IGameMakerChunk chunk) {
        console.Output.WriteLine($"    Chunk \"{chunk.Name}\" ({chunk.GetType().FullName})");
        console.Output.WriteLine($"    Size: {chunk.Size} bytes");
    }
}
