using System.Text;

namespace Tomat.GameMaker.IFF.IO;

public sealed class GameMakerIffWriter : IGameMakerIffDataHandler {
    public const int DEFAULT_CAPACITY = 1024 * 1024 * 10; // 10 MB

    public Encoding Encoding { get; }

    public byte[] Data { get; }

    public long Position { get; set; }

    public long Length { get; set; }

    public GameMakerIffWriter(int capacity = DEFAULT_CAPACITY, Encoding? encoding = null) {
        Data = new byte[capacity];
        Position = 0;
        Length = 0;
        Encoding = encoding ?? IGameMakerIffDataHandler.DEFAULT_ENCODING;
    }
}
