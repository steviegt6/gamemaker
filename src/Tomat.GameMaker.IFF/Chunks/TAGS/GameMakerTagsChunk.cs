using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AssetTag;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.TAGS;

public sealed class GameMakerTagsChunk : AbstractChunk {
    public const string NAME = "TAGS";

    public List<GameMakerPointer<GameMakerString>>? Tags { get; set; }

    public GameMakerPointerList<GameMakerAssetTag>? AssetTags { get; set; }

    public GameMakerTagsChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        context.Reader.Pad(4);

        var chunkVersion = context.Reader.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {chunkVersion}.");

        var count = context.Reader.ReadInt32();
        Tags = new List<GameMakerPointer<GameMakerString>>(count);
        for (var i = count; i > 0; i--)
            Tags.Add(context.ReadPointerAndObject<GameMakerString>());

        AssetTags = new GameMakerPointerList<GameMakerAssetTag>();
        AssetTags.Read(context);
    }

    public override void Write(SerializationContext context) {
        context.Writer.Pad(4);
        context.Writer.Write(1);

        context.Writer.Write(Tags!.Count);
        foreach (var tag in Tags)
            context.Writer.Write(tag);

        AssetTags!.Write(context);
    }
}
