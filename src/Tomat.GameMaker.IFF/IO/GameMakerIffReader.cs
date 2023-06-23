using System.Text;

namespace Tomat.GameMaker.IFF.IO;

public sealed class GameMakerIffReader : IGameMakerIffDataHandler {
    public Encoding Encoding { get; }

    public byte[] Data { get; }

    public long Position { get; set; }

    public long Length => Data.Length;
    
    public GameMakerIffReader(byte[] data, Encoding? encoding = null) {
        Data = data;
        Position = 0;
        Encoding = encoding ?? IGameMakerIffDataHandler.DEFAULT_ENCODING;
    }
}
