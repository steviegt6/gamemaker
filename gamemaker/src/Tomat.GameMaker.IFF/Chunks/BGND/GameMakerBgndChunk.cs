using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Background;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.BGND;

internal sealed class GameMakerBgndChunk : AbstractChunk,
                                           IBgndChunk {
    public const string NAME = "BGND";

    public GameMakerPointerList<GameMakerBackground> Backgrounds { get; set; } = null!;

    public GameMakerBgndChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Backgrounds = context.ReadPointerList<GameMakerBackground>(
            elementReader: (ctx, notLast) => {
                var ptr = ctx.ReadInt32();

                if (context.Position % context.VersionInfo.BackgroundAlignment != 0)
                    throw new InvalidDataException($"Expected background pointer to be aligned to {context.VersionInfo.BackgroundAlignment} bytes, got {context.Position % context.VersionInfo.BackgroundAlignment}.");

                return ctx.ReadPointerAndObject<GameMakerBackground>(ptr, returnAfter: notLast);
            }
        );
    }

    public override void Write(SerializationContext context) {
        context.Write(
            Backgrounds,
            beforeWriter: (ctx, _, _) => {
                ctx.Pad(context.VersionInfo.BackgroundAlignment);
            }
        );
    }
}
