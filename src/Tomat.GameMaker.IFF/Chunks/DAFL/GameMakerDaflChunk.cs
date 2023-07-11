namespace Tomat.GameMaker.IFF.Chunks.DAFL;

// Empty.
public sealed class GameMakerDaflChunk : AbstractChunk {
    public const string NAME = "DAFL";

    public GameMakerDaflChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) { }

    public override void Write(SerializationContext context) { }
}
