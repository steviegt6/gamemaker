namespace Tomat.GameMaker.IFF.Chunks;

// TODO: Implement this chunk.

/// <summary>
///     The <c>DAFL</c> chunk, which is currently empty, but may be used for
///     data files in the future.
/// </summary>
public interface IDaflChunk : IGameMakerChunk { }

internal sealed class GameMakerDaflChunk : AbstractChunk,
                                           IDaflChunk {
    public const string NAME = "DAFL";

    public GameMakerDaflChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) { }

    public override void Write(SerializationContext context) { }
}
