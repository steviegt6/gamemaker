namespace Tomat.GameMaker.IFF;

/// <summary>
///     Represents a chunk within an IFF file structure.
/// </summary>
public interface IGameMakerChunk : IGameMakerSerializableWithComponents {
    public const int NAME_LENGTH = 4;

    /// <summary>
    ///     The name of the chunk.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The size of the chunk, in bytes.
    /// </summary>
    int Size { get; set; }

    /// <summary>
    ///     The position of the chunk, in bytes.
    /// </summary>
    int StartPosition { get; set; }

    /// <summary>
    ///     The position of the chunk, in bytes, relative to the end of the file.
    /// </summary>
    int EndPosition => StartPosition + Size;
}
