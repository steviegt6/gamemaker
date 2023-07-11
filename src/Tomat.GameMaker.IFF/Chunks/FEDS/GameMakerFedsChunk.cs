using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.FilterEffect;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.FEDS;

public sealed class GameMakerFedsChunk : AbstractChunk {
    public const string NAME = "FEDS";

    public GameMakerPointerList<GameMakerFilterEffect>? FilterEffects { get; set; }

    public GameMakerFedsChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        context.Pad(4);

        var chunkVersion = context.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {chunkVersion}.");

        FilterEffects = new GameMakerPointerList<GameMakerFilterEffect>();
        FilterEffects.Read(context);
    }

    public override void Write(SerializationContext context) {
        context.Pad(4);
        context.Write(1);
        FilterEffects!.Write(context);
    }
}
