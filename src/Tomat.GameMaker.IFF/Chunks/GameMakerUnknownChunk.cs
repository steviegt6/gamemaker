using System.IO;
using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.Chunks; 

public sealed class GameMakerUnknownChunk : IGameMakerChunk {
    public string Name { get; }

    public int Size { get; set; }
    
    public byte[]? Data { get; set; }

    public GameMakerUnknownChunk(string name, int size) {
        Name = name;
        Size = size;
    }
    
    public int Read(DeserializationContext context) {
        Data = context.Reader.ReadBytes(Size).ToArray();
        return Data.Length; // Size
    }

    public int Write(SerializationContext context) {
        if (Data is null)
            throw new IOException("Cannot write unknown chunk without data.");
        
        Size = Data.Length;
        context.Writer.Write(Data);
        return Data.Length; // Size
    }
}
