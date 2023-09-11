using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.FilterEffect;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.FEDS;

internal sealed class GameMakerFedsChunk : AbstractChunk,
                                           IFedsChunk {
    public const string NAME = "FEDS";

    public int ChunkVersion { get; set; }

    public GameMakerPointerList<GameMakerFilterEffect> FilterEffects { get; set; } = null!;

    public GameMakerFedsChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.Align(4);

        ChunkVersion = context.ReadInt32();
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        FilterEffects = context.ReadPointerList<GameMakerFilterEffect>();
    }

    public override void Write(SerializationContext context) {
        context.Align(4);

        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        context.Write(ChunkVersion);
        context.Write(FilterEffects);
    }
}
