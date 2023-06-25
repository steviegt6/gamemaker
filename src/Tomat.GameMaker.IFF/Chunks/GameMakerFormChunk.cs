using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     The FORM chunk making up the entire GameMaker IFF file. This chunk may
///     contain many smaller chunks, as it contains its own IFF file structure.
/// </summary>
public sealed class GameMakerFormChunk : IGameMakerChunk {
    public const string NAME = "FORM";

    public delegate IGameMakerChunk ChunkFactory(string name, int size);

    public string Name { get; }

    public int Size { get; set; }

    public Dictionary<string, IGameMakerChunk>? Chunks { get; set; }

    public Dictionary<string, ChunkFactory> ChunkFactories { get; }

    public ChunkFactory DefaultChunkFactory { get; }

    public GameMakerFormChunk(string name, int size) {
        Name = name;
        Size = size;
        ChunkFactories = new Dictionary<string, ChunkFactory> { };
        DefaultChunkFactory = (chunkName, chunkSize) => new GameMakerUnknownChunk(chunkName, chunkSize);
    }

    public int Read(DeserializationContext context) {
        var startPosition = context.Reader.Position;
        var foundChunks = new Dictionary<(string, int), int>();

        while (context.Reader.Position < startPosition + Size) {
            var chunkName = context.Reader.ReadChars(IGameMakerChunk.NAME_LENGTH);
            var chunkSize = context.Reader.ReadInt32();
            foundChunks.Add((new string(chunkName), chunkSize), context.Reader.Position);
            context.Reader.Position += chunkSize;
        }

        Chunks = new Dictionary<string, IGameMakerChunk>();

        foreach (var ((chunkName, chunkLength), chunkPos) in foundChunks) {
            context.Reader.Position = chunkPos;
            Chunks[chunkName] = ChunkFactories.TryGetValue(chunkName, out var factory)
                ? factory(chunkName, chunkLength)
                : DefaultChunkFactory(chunkName, chunkLength);
            var readSize = Chunks[chunkName].Read(context);
            if (readSize != chunkLength)
                throw new IOException($"Chunk {chunkName} size does not match actual size.");
        }

        return Size;
    }

    public int Write(SerializationContext context) {
        if (Chunks is null)
            throw new IOException("Cannot write FORM chunk without any sub-chunks.");

        Size = 0;
        foreach (var chunk in Chunks.Values)
            Size += chunk.Write(context);

        return Size;
    }
}
