namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents a chunk within an IFF file structure.
/// </summary>
public interface IGameMakerChunk : IGameMakerSerializable {
    public const int NAME_LENGTH = 4;

    /// <summary>
    ///     The name of the chunk.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The size of the chunk, in bytes.
    /// </summary>
    int Size { get; set; }
}
