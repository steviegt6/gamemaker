using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.Chunks.AUDO;

public sealed class GameMakerAudoChunk : IGameMakerChunk {
    public const string NAME = "AUDO";

    public string Name { get; }

    public int Size { get; }

    public GameMakerAudoChunk(string name, int size) {
        Name = name;
        Size = size;
    }

    public void Read(DeserializationContext context) {
        throw new System.NotImplementedException();
    }

    public void Write(SerializationContext context) {
        throw new System.NotImplementedException();
    }
}
