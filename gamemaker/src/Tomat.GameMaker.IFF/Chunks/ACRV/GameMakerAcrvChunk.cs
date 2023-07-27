using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

namespace Tomat.GameMaker.IFF.Chunks.ACRV;

/// <summary>
///     <c>ACRV</c> chunk
/// </summary>
internal sealed class GameMakerAcrvChunk : AbstractChunk,
                                           IAcrvChunk {
    public const string NAME = "ACRV";

    public int ChunkVersion { get; set; }

    public GameMakerPointerList<GameMakerAnimationCurve> AnimationCurves { get; set; } = null!;

    public GameMakerAcrvChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        ChunkVersion = context.ReadInt32();
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        AnimationCurves = context.ReadPointerList<GameMakerAnimationCurve>();
    }

    public override void Write(SerializationContext context) {
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        context.Write(ChunkVersion);
        context.Write(AnimationCurves);
    }
}
