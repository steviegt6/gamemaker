using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AssetTag;

public sealed class GameMakerAssetTag : IGameMakerSerializable {
    public int Id { get; set; }

    public List<GameMakerPointer<GameMakerString>>? Tags { get; set; }

    public void Read(DeserializationContext context) {
        Id = context.Reader.ReadInt32();
        var count = context.Reader.ReadInt32();
        Tags = new List<GameMakerPointer<GameMakerString>>();
        for (var i = count; i > 0; i--)
            Tags.Add(context.ReadPointerAndObject<GameMakerString>());
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Id);
        context.Writer.Write(Tags!.Count);
        foreach (var tag in Tags)
            context.Writer.Write(tag);
    }
}
