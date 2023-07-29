using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Tomat.GameMaker.IFF.Chunks.ACRV;
using Tomat.GameMaker.IFF.Chunks.AGRP;
using Tomat.GameMaker.IFF.Chunks.AUDO;
using Tomat.GameMaker.IFF.Chunks.BGND;
using Tomat.GameMaker.IFF.Chunks.CODE;
using Tomat.GameMaker.IFF.Chunks.DAFL;
using Tomat.GameMaker.IFF.Chunks.EMBI;
using Tomat.GameMaker.IFF.Chunks.EXTN;
using Tomat.GameMaker.IFF.Chunks.FEAT;
using Tomat.GameMaker.IFF.Chunks.FEDS;
using Tomat.GameMaker.IFF.Chunks.FONT;
using Tomat.GameMaker.IFF.Chunks.FUNC;
using Tomat.GameMaker.IFF.Chunks.GEN8;
using Tomat.GameMaker.IFF.Chunks.GLOB;
using Tomat.GameMaker.IFF.Chunks.GMEN;
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
using Tomat.GameMaker.IFF.Chunks.TGIN;
using Tomat.GameMaker.IFF.Chunks.TMLN;
using Tomat.GameMaker.IFF.Chunks.TPAG;
using Tomat.GameMaker.IFF.Chunks.TXTR;
using Tomat.GameMaker.IFF.Chunks.VARI;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.FORM;

/// <summary>
///     The FORM chunk making up the entire GameMaker IFF file. This chunk may
///     contain many smaller chunks, as it contains its own IFF file structure.
/// </summary>
internal sealed class GameMakerFormChunk : IFormChunk {
    public const string NAME = "FORM";

    public delegate IGameMakerChunk ChunkFactory(string name, int size, int startPosition);

    public Dictionary<Type, object> Components { get; } = new();

    public string Name { get; }

    public int Size { get; set; }

    public int StartPosition { get; set; }

    public Dictionary<string, IGameMakerChunk> Chunks { get; set; } = null!;

    public Dictionary<string, ChunkFactory> ChunkFactories { get; }

    public ChunkFactory DefaultChunkFactory { get; }

    public GameMakerFormChunk(string name, int size, int startPosition) {
        Name = name;
        Size = size;
        StartPosition = startPosition;
        ChunkFactories = new Dictionary<string, ChunkFactory> {
            { GameMakerGen8Chunk.NAME, (c, s, p) => new GameMakerGen8Chunk(c, s, p) },
            { GameMakerOptnChunk.NAME, (c, s, p) => new GameMakerOptnChunk(c, s, p) },
            { GameMakerLangChunk.NAME, (c, s, p) => new GameMakerLangChunk(c, s, p) },
            { GameMakerExtnChunk.NAME, (c, s, p) => new GameMakerExtnChunk(c, s, p) },
            { GameMakerSondChunk.NAME, (c, s, p) => new GameMakerSondChunk(c, s, p) },
            { GameMakerAgrpChunk.NAME, (c, s, p) => new GameMakerAgrpChunk(c, s, p) },
            { GameMakerSprtChunk.NAME, (c, s, p) => new GameMakerSprtChunk(c, s, p) },
            { GameMakerBgndChunk.NAME, (c, s, p) => new GameMakerBgndChunk(c, s, p) },
            { GameMakerPathChunk.NAME, (c, s, p) => new GameMakerPathChunk(c, s, p) },
            { GameMakerScptChunk.NAME, (c, s, p) => new GameMakerScptChunk(c, s, p) },
            { GameMakerGlobChunk.NAME, (c, s, p) => new GameMakerGlobChunk(c, s, p) },
            { GameMakerGmenChunk.NAME, (c, s, p) => new GameMakerGmenChunk(c, s, p) },
            { GameMakerShdrChunk.NAME, (c, s, p) => new GameMakerShdrChunk(c, s, p) },
            { GameMakerFontChunk.NAME, (c, s, p) => new GameMakerFontChunk(c, s, p) },
            { GameMakerTmlnChunk.NAME, (c, s, p) => new GameMakerTmlnChunk(c, s, p) },
            { GameMakerObjtChunk.NAME, (c, s, p) => new GameMakerObjtChunk(c, s, p) },
            { GameMakerFedsChunk.NAME, (c, s, p) => new GameMakerFedsChunk(c, s, p) },
            { GameMakerAcrvChunk.NAME, (c, s, p) => new GameMakerAcrvChunk(c, s, p) },
            { GameMakerSeqnChunk.NAME, (c, s, p) => new GameMakerSeqnChunk(c, s, p) },
            { GameMakerTagsChunk.NAME, (c, s, p) => new GameMakerTagsChunk(c, s, p) },
            { GameMakerRoomChunk.NAME, (c, s, p) => new GameMakerRoomChunk(c, s, p) },
            { GameMakerDaflChunk.NAME, (c, s, p) => new GameMakerDaflChunk(c, s, p) },
            { GameMakerEmbiChunk.NAME, (c, s, p) => new GameMakerEmbiChunk(c, s, p) },
            { GameMakerTpagChunk.NAME, (c, s, p) => new GameMakerTpagChunk(c, s, p) },
            { GameMakerTginChunk.NAME, (c, s, p) => new GameMakerTginChunk(c, s, p) },
            { GameMakerCodeChunk.NAME, (c, s, p) => new GameMakerCodeChunk(c, s, p) },
            { GameMakerVariChunk.NAME, (c, s, p) => new GameMakerVariChunk(c, s, p) },
            { GameMakerFuncChunk.NAME, (c, s, p) => new GameMakerFuncChunk(c, s, p) },
            { GameMakerFeatChunk.NAME, (c, s, p) => new GameMakerFeatChunk(c, s, p) },
            { GameMakerStrgChunk.NAME, (c, s, p) => new GameMakerStrgChunk(c, s, p) },
            { GameMakerTxtrChunk.NAME, (c, s, p) => new GameMakerTxtrChunk(c, s, p) },
            { GameMakerAudoChunk.NAME, (c, s, p) => new GameMakerAudoChunk(c, s, p) },
        };
        DefaultChunkFactory = (c, s, p) => new GameMakerUnknownChunk(c, s, p);
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
                ? factory(chunkName, chunkLength, chunkPos)
                : DefaultChunkFactory(chunkName, chunkLength, chunkPos);
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

            chunk.Size = context.EndLength(chunkLengthPosition);
            i++;
        }
    }

    public bool TryGetComponent<T>([NotNullWhen(returnValue: true)] out T? component) where T : class {
        component = null;
        return false;
    }

    public void AddComponent<T>(T component) where T : class { }

    public void AddComponent(Type type, object component) { }
}
