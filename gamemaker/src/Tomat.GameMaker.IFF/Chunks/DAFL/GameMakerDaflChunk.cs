namespace Tomat.GameMaker.IFF.Chunks.DAFL;

internal sealed class GameMakerDaflChunk : AbstractChunk,
                                           IDaflChunk {
    public const string NAME = "DAFL";

    public GameMakerDaflChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) { }

    public override void Write(SerializationContext context) { }
}
