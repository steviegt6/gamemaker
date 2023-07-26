using System;
using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents a chunk within an IFF file structure.
/// </summary>
public interface IGameMakerChunk : IGameMakerSerializable {
    public const int NAME_LENGTH = 4;

    Dictionary<Type, object> ChunkComponents { get; }

    /// <summary>
    ///     The name of the chunk.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The size of the chunk, in bytes.
    /// </summary>
    int Size { get; set; }

    bool TryGetComponent<T>(out T? component) where T : class;

    void AddComponent<T>(T component) where T : class;

    void AddComponent(Type type, object component);
}
