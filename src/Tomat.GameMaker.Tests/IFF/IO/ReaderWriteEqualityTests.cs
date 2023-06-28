using System.IO;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks.Contexts;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.Tests.IFF.IO;

[TestFixture]
public static class ReaderWriteEqualityTests {
    public static void TestEquality() {
        var stream = File.Open("test.wad", FileMode.Open, FileAccess.Read, FileShare.Read);
        var wad = GameMakerIffFile.FromStream(stream, out var deserCtx);
        var readBytes = deserCtx.Reader.Data;
        var writer = new GameMakerIffWriter(wad.Form!.Size + 8);
        wad.Write(new SerializationContext(writer, wad, deserCtx.VersionInfo));
        var writtenBytes = writer.Data;

        Assert.AreEqual(readBytes.Length, writtenBytes.Length);
        CollectionAssert.AreEqual(readBytes, writtenBytes);
    }
}
