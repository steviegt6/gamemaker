using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     The <c>ACRV</c> chunk, which contains animation curves.
/// </summary>
public interface IAcrvChunk : IGameMakerChunk {
    /// <summary>The version of the chunk.</summary>
    /// <remarks>Expected to always be <c>1</c>.</remarks>
    int ChunkVersion { get; }

    /// <summary>The list of animation curves.</summary>
    List<GameMakerPointer<IAnimationCurve>> AnimationCurves { get; set; }
}

internal sealed class GameMakerAcrvChunk : AbstractChunk,
                                           IAcrvChunk {
    public const string NAME = "ACRV";
    public const int VERSION = 1;

    public int ChunkVersion { get; set; }

    public List<GameMakerPointer<IAnimationCurve>> AnimationCurves { get; set; } = null!;

    public GameMakerAcrvChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.GmAlign(4);
        ChunkVersion = context.ReadInt32().Expect(VERSION, x => new InvalidDataException($"Expected chunk version 1, got {x}."));
        AnimationCurves = context.ReadPointerList<IAnimationCurve, GameMakerAnimationCurve>(afterRead: (ctx, _, _) => ctx.GmAlign(4));
    }

    public override void Write(SerializationContext context) {
        context.GmAlign(4);
        context.Write(ChunkVersion.Expect(VERSION, x => new InvalidDataException($"Expected chunk version 1, got {x}.")));
        context.WritePointerList(AnimationCurves, afterWrite: (ctx, _, _) => ctx.GmAlign(4));
    }
}
