using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks.AGRP;
using Tomat.GameMaker.IFF.Chunks.ARV;
using Tomat.GameMaker.IFF.Chunks.AUDO;
using Tomat.GameMaker.IFF.Chunks.BGND;
using Tomat.GameMaker.IFF.Chunks.DAFL;
using Tomat.GameMaker.IFF.Chunks.EMBI;
using Tomat.GameMaker.IFF.Chunks.EXTN;
using Tomat.GameMaker.IFF.Chunks.FEAT;
using Tomat.GameMaker.IFF.Chunks.FEDS;
using Tomat.GameMaker.IFF.Chunks.FONT;
using Tomat.GameMaker.IFF.Chunks.GEN8;
using Tomat.GameMaker.IFF.Chunks.GLOB;
using Tomat.GameMaker.IFF.Chunks.LANG;
using Tomat.GameMaker.IFF.Chunks.OBJT;
using Tomat.GameMaker.IFF.Chunks.OPTN;
using Tomat.GameMaker.IFF.Chunks.PATH;
using Tomat.GameMaker.IFF.Chunks.ROOM;
using Tomat.GameMaker.IFF.Chunks.SCPT;
using Tomat.GameMaker.IFF.Chunks.SEQN;
using Tomat.GameMaker.IFF.Chunks.SHDR;
using Tomat.GameMaker.IFF.Chunks.SOND;
using Tomat.GameMaker.IFF.Chunks.SPRT;
using Tomat.GameMaker.IFF.Chunks.STRG;
using Tomat.GameMaker.IFF.Chunks.TAGS;
using Tomat.GameMaker.IFF.Chunks.TMLN;
using Tomat.GameMaker.IFF.Chunks.TXTR;
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
            { GameMakerOptnChunk.NAME, (c, s) => new GameMakerOptnChunk(c, s) },
            { GameMakerLangChunk.NAME, (c, s) => new GameMakerLangChunk(c, s) },
            { GameMakerExtnChunk.NAME, (c, s) => new GameMakerExtnChunk(c, s) },
            { GameMakerSondChunk.NAME, (c, s) => new GameMakerSondChunk(c, s) },
            { GameMakerAgrpChunk.NAME, (c, s) => new GameMakerAgrpChunk(c, s) },
            { GameMakerSprtChunk.NAME, (c, s) => new GameMakerSprtChunk(c, s) },
            { GameMakerBgndChunk.NAME, (c, s) => new GameMakerBgndChunk(c, s) },
            { GameMakerPathChunk.NAME, (c, s) => new GameMakerPathChunk(c, s) },
            { GameMakerScptChunk.NAME, (c, s) => new GameMakerScptChunk(c, s) },
            { GameMakerGlobChunk.NAME, (c, s) => new GameMakerGlobChunk(c, s) },
            { GameMakerShdrChunk.NAME, (c, s) => new GameMakerShdrChunk(c, s) },
            { GameMakerFontChunk.NAME, (c, s) => new GameMakerFontChunk(c, s) },
            { GameMakerTmlnChunk.NAME, (c, s) => new GameMakerTmlnChunk(c, s) },
            { GameMakerObjtChunk.NAME, (c, s) => new GameMakerObjtChunk(c, s) },
            { GameMakerFedsChunk.NAME, (c, s) => new GameMakerFedsChunk(c, s) },
            { GameMakerAcrvChunk.NAME, (c, s) => new GameMakerAcrvChunk(c, s) },
            { GameMakerSeqnChunk.NAME, (c, s) => new GameMakerSeqnChunk(c, s) },
            { GameMakerTagsChunk.NAME, (c, s) => new GameMakerTagsChunk(c, s) },
            { GameMakerRoomChunk.NAME, (c, s) => new GameMakerRoomChunk(c, s) },
            { GameMakerDaflChunk.NAME, (c, s) => new GameMakerDaflChunk(c, s) },
            { GameMakerEmbiChunk.NAME, (c, s) => new GameMakerEmbiChunk(c, s) },
            // { GameMakerTpagChunk.NAME, (c, s) => new GameMakerTpagChunk(c, s) },
            // { GameMakerTginChunk.NAME, (c, s) => new GameMakerTginChunk(c, s) },
            // { GameMakerCodeChunk.NAME, (c, s) => new GameMakerCodeChunk(c, s) },
            // { GameMakerVariChunk.NAME, (c, s) => new GameMakerVariChunk(c, s) },
            // { GameMakerFuncChunk.NAME, (c, s) => new GameMakerFuncChunk(c, s) },
            { GameMakerFeatChunk.NAME, (c, s) => new GameMakerFeatChunk(c, s) },
            { GameMakerStrgChunk.NAME, (c, s) => new GameMakerStrgChunk(c, s) },
            { GameMakerTxtrChunk.NAME, (c, s) => new GameMakerTxtrChunk(c, s) },
            { GameMakerAudoChunk.NAME, (c, s) => new GameMakerAudoChunk(c, s) },
        };
        DefaultChunkFactory = (c, s) => new GameMakerUnknownChunk(c, s);
    }

    public void Read(DeserializationContext context) {
        var startPosition = context.Position;
        var foundChunks = new Dictionary<(string, int), int>();

        while (context.Position < startPosition + Size) {
            var chunkName = new string(context.ReadChars(IGameMakerChunk.NAME_LENGTH));
            var chunkSize = context.ReadInt32();
            foundChunks.Add((chunkName, chunkSize), context.Position);
            context.Position += chunkSize;

            if (chunkName == "SEQN")
                context.VersionInfo.UpdateTo(GM_2_3);
            else if (chunkName == "FEDS")
                context.VersionInfo.UpdateTo(GM_2_3_6);
            else if (chunkName == "FEAT")
                context.VersionInfo.UpdateTo(GM_2022_8);

            // Final chunk isn't aligned, which we can infer by whether the end
            // of the chunk is also the end of the file.
            if (context.Position % context.VersionInfo.ChunkAlignment != 0 && context.Position < context.Length)
                throw new IOException("Chunk alignment setting does not match actual chunk alignment!");
        }

        Chunks = new Dictionary<string, IGameMakerChunk>();

        foreach (var ((chunkName, chunkLength), chunkPos) in foundChunks) {
            context.Position = chunkPos;
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
            context.Write(chunk.Name.ToCharArray());
            var chunkLengthPosition = context.BeginLength();
            chunk.Write(context);

            if (i != Chunks.Count - 1)
                context.Pad(context.VersionInfo.ChunkAlignment);

            context.EndLength(chunkLengthPosition);
            i++;
        }
    }
}
