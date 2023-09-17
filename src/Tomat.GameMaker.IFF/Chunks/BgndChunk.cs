using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Background;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     The <c>BGND</c> chunk, which contains backgrounds.
/// </summary>
public interface IBgndChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of backgrounds.
    /// </summary>
    List<GameMakerPointer<IBackground>> Backgrounds { get; set; }
}

internal sealed class GameMakerBgndChunk : AbstractChunk,
                                           IBgndChunk {
    public const string NAME = "BGND";

    public List<GameMakerPointer<IBackground>> Backgrounds { get; set; } = null!;

    public GameMakerBgndChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Backgrounds = context.ReadPointerList<IBackground, GameMakerBackground>(
            readElement: (ctx, notLast) => {
                var ptr = ctx.ReadInt32();

                if (context.Position % context.VersionInfo.BackgroundAlignment != 0)
                    throw new InvalidDataException($"Expected background pointer to be aligned to {context.VersionInfo.BackgroundAlignment} bytes, got {context.Position % context.VersionInfo.BackgroundAlignment}.");

                return ctx.ReadPointerAndObject<IBackground, GameMakerBackground>(ptr, returnAfter: notLast);
            }
        );
    }

    public override void Write(SerializationContext context) {
        context.WritePointerList(
            Backgrounds,
            beforeWrite: (ctx, _, _) => {
                ctx.GmAlign(context.VersionInfo.BackgroundAlignment);
            }
        );
    }
}
