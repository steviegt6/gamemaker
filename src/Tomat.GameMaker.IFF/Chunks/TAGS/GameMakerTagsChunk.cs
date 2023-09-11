using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AssetTag;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.TAGS;

internal sealed class GameMakerTagsChunk : AbstractChunk,
                                           ITagsChunk {
    public const string NAME = "TAGS";

    public int ChunkVersion { get; set; }

    public List<GameMakerPointer<GameMakerString>> Tags { get; set; } = null!;

    public GameMakerPointerList<GameMakerAssetTag> AssetTags { get; set; } = null!;

    public GameMakerTagsChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.Align(4);

        ChunkVersion = context.ReadInt32();
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        var count = context.ReadInt32();
        Tags = new List<GameMakerPointer<GameMakerString>>(count);
        for (var i = count; i > 0; i--)
            Tags.Add(context.ReadPointerAndObject<GameMakerString>());

        AssetTags = context.ReadPointerList<GameMakerAssetTag>();
    }

    public override void Write(SerializationContext context) {
        context.Align(4);

        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        context.Write(ChunkVersion);

        context.Write(Tags!.Count);
        foreach (var tag in Tags)
            context.Write(tag);

        context.Write(AssetTags);
    }
}
