using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.ACRV;

internal sealed class GameMakerAcrvChunk : AbstractChunk,
                                           IAcrvChunk {
    public const string NAME = "ACRV";
    public const int VERSION = 1;

    public int ChunkVersion { get; set; }

    public List<GameMakerPointer<IAnimationCurve>> AnimationCurves { get; set; } = null!;

    public GameMakerAcrvChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.GmAlign(4);
        ChunkVersion = context.ReadInt32().Expect(VERSION, new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}."));
        AnimationCurves = context.ReadPointerList<IAnimationCurve, GameMakerAnimationCurve>(afterRead: (ctx, _, _) => ctx.GmAlign(4));
    }

    public override void Write(SerializationContext context) {
        context.GmAlign(4);
        context.Write(ChunkVersion.Expect(VERSION, new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.")));
        context.WritePointerList(AnimationCurves, afterWrite: (ctx, _, _) => ctx.GmAlign(4));
    }
}
