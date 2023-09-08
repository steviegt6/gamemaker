using System;
using System.IO;
using System.Linq;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.Tests.IFF.IO;

[TestFixture]
public static class ReaderWriteEqualityTests {
    public static void TestEquality() {
        using var stream = File.Open("test.wad", FileMode.Open, FileAccess.Read, FileShare.Read);
        var wad = GameMakerIffFile.FromStream(stream, out var deserCtx);
        var readBytes = deserCtx.Data;
        var writer = new GameMakerIffWriter(wad.Form!.Size + 8);
        wad.Write(new SerializationContext(writer, wad, deserCtx.VersionInfo));
        var writtenBytes = writer.Data;

        Assert.That(writtenBytes, Has.Length.EqualTo(readBytes.Length));
        CollectionAssert.AreEqual(readBytes, writtenBytes);
    }

    public static void ExhaustiveChunkTest() {
        using var stream = File.Open("test.wad", FileMode.Open, FileAccess.Read, FileShare.Read);
        var wad = GameMakerIffFile.FromStream(stream, out var deserCtx);

        foreach (var chunk in wad.Form.Chunks) {
            Console.WriteLine($"{chunk.Key} ({chunk.Value.Name}, {chunk.Value.Size}) ({chunk.Value.GetType().FullName})");
            
            if (chunk.Value is GameMakerUnknownChunk)
                throw new Exception("Unknown chunk!");
        }
    }

    public static void TestThisIsForDebuggingIgnoreLol() {
        var stream = File.Open("test.wad", FileMode.Open, FileAccess.Read, FileShare.Read);
        var wad = GameMakerIffFile.FromStream(stream, out var deserCtx);
        var readBytes = deserCtx.Data;
        var chunkLengths = wad.Form!.Chunks!.ToDictionary(x => x.Key, x => x.Value.Size);
        var chunkSlices = wad.Form.Chunks.ToDictionary(x => x.Key, x => readBytes[x.Value.StartPosition..x.Value.EndPosition]);
        var writer = new GameMakerIffWriter(wad.Form!.Size + 8);
        wad.Write(new SerializationContext(writer, wad, deserCtx.VersionInfo));
        var writtenBytes = writer.Data;

        //foreach (var chunkName in chunkLengths.Keys)
        //    Assert.That(chunkLengths[chunkName], Is.EqualTo(wad.Form.Chunks![chunkName].Size));
        //Assert.That(writer, Has.Length.EqualTo(readBytes.Length));

        foreach (var chunkName in wad.Form.Chunks.Keys) {
            Console.WriteLine(chunkName);
            var currChunk = wad.Form.Chunks[chunkName];
            Assert.That(currChunk.Size, Is.EqualTo(chunkLengths[chunkName]));
            //CollectionAssert.AreEqual(chunkSlices[chunkName], writtenBytes[currChunk.StartPosition..currChunk.EndPosition]);
        }

        using var newStream = new MemoryStream(writtenBytes);
        newStream.Seek(0, SeekOrigin.Begin);
        var newWad = GameMakerIffFile.FromStream(newStream, out var newDeserCtx);
        var newReadBytes = newDeserCtx.Data;
        var newWriter = new GameMakerIffWriter(newWad.Form!.Size + 8);
        newWad.Write(new SerializationContext(newWriter, newWad, newDeserCtx.VersionInfo));
        var newWrittenBytes = newWriter.Data;

        Assert.That(newWrittenBytes, Has.Length.EqualTo(newReadBytes.Length));
    }
}
