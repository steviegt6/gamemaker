using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks.Contexts;
using Tomat.GameMaker.IFF.Chunks.GEN8;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.FORM;

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
        ChunkFactories = new Dictionary<string, ChunkFactory> {
            { GameMakerGen8Chunk.NAME, (c, s) => new GameMakerGen8Chunk(c, s) },
        };
        DefaultChunkFactory = (c, s) => new GameMakerUnknownChunk(c, s);
    }

    public void Read(DeserializationContext context) {
        var startPosition = context.Reader.Position;
        var foundChunks = new Dictionary<(string, int), int>();

        while (context.Reader.Position < startPosition + Size) {
            var chunkName = new string(context.Reader.ReadChars(IGameMakerChunk.NAME_LENGTH));
            var chunkSize = context.Reader.ReadInt32();
            foundChunks.Add((chunkName, chunkSize), context.Reader.Position);
            context.Reader.Position += chunkSize;

            if (chunkName == "SEQN")
                context.VersionInfo.Update(GameMakerVersionInfo.GM_2_3);
            else if (chunkName == "FEDS")
                context.VersionInfo.Update(GameMakerVersionInfo.GM_2_3_6);
            else if (chunkName == "FEAT")
                context.VersionInfo.Update(GameMakerVersionInfo.GM_2022_8);

            // Final chunk isn't aligned, which we can infer by whether the end
            // of the chunk is also the end of the file.
            if (context.Reader.Position % context.VersionInfo.ChunkAlignment != 0 && context.Reader.Position < context.Reader.Length)
                throw new IOException("Chunk alignment setting does not match actual chunk alignment!");
        }

        Chunks = new Dictionary<string, IGameMakerChunk>();

        foreach (var ((chunkName, chunkLength), chunkPos) in foundChunks) {
            context.Reader.Position = chunkPos;
            Chunks[chunkName] = ChunkFactories.TryGetValue(chunkName, out var factory)
                ? factory(chunkName, chunkLength)
                : DefaultChunkFactory(chunkName, chunkLength);
            Chunks[chunkName].Read(context);
        }
    }

    public void Write(SerializationContext context) {
        if (Chunks is null)
            throw new IOException("Cannot write FORM chunk without any sub-chunks.");

        var i = 0;

        foreach (var chunk in Chunks.Values) {
            context.Writer.Write(chunk.Name.ToCharArray());
            var chunkLengthPosition = context.Writer.BeginLength();
            chunk.Write(context);

            if (i != Chunks.Count - 1)
                context.Writer.Pad(context.VersionInfo.ChunkAlignment);

            context.Writer.EndLength(chunkLengthPosition);
            i++;
        }
    }
}
