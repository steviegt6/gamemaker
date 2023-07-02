using System.IO;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.Tests.IFF.IO;

[TestFixture]
public static class ReaderWriteEqualityTests {
    public static void TestEquality() {
        using var stream = File.Open("test.wad", FileMode.Open, FileAccess.Read, FileShare.Read);
        var wad = GameMakerIffFile.FromStream(stream, out var deserCtx);
        var readBytes = deserCtx.Reader.Data;
        var writer = new GameMakerIffWriter(wad.Form!.Size + 8);
        wad.Write(new SerializationContext(writer, wad, deserCtx.VersionInfo));
        var writtenBytes = writer.Data;

        Assert.That(writtenBytes, Has.Length.EqualTo(readBytes.Length));
        CollectionAssert.AreEqual(readBytes, writtenBytes);
    }

    public static void TestThisIsForDebuggingIgnoreLol() {
        var stream = File.Open("test.wad", FileMode.Open, FileAccess.Read, FileShare.Read);
        var wad = GameMakerIffFile.FromStream(stream, out var deserCtx);
        var readBytes = deserCtx.Reader.Data;
        var writer = new GameMakerIffWriter(wad.Form!.Size + 8);
        wad.Write(new SerializationContext(writer, wad, deserCtx.VersionInfo));
        var writtenBytes = writer.Data;

        Assert.That(writtenBytes, Has.Length.EqualTo(readBytes.Length));

        using var newStream = new MemoryStream(writtenBytes);
        newStream.Seek(0, SeekOrigin.Begin);
        var newWad = GameMakerIffFile.FromStream(newStream, out var newDeserCtx);
        var newReadBytes = newDeserCtx.Reader.Data;
        var newWriter = new GameMakerIffWriter(newWad.Form!.Size + 8);
        newWad.Write(new SerializationContext(newWriter, newWad, newDeserCtx.VersionInfo));
        var newWrittenBytes = newWriter.Data;
        
        Assert.That(newWrittenBytes, Has.Length.EqualTo(newReadBytes.Length));
    }
}
