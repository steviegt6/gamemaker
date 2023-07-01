using System.IO;

namespace Tomat.GameMaker.IFF.Chunks;

public sealed class GameMakerUnknownChunk : IGameMakerChunk {
    public string Name { get; }

    public int Size { get; set; }

    public byte[]? Data { get; set; }

    public GameMakerUnknownChunk(string name, int size) {
        Name = name;
        Size = size;
    }

    public void Read(DeserializationContext context) {
        Data = context.Reader.ReadBytes(Size).ToArray();
    }

    public void Write(SerializationContext context) {
        if (Data is null)
            throw new IOException("Cannot write unknown chunk without data.");

        context.Writer.Write(Data);
    }
}
