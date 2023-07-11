using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Background;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.BGND;

public sealed class GameMakerBgndChunk : AbstractChunk {
    public const string NAME = "BGND";

    public GameMakerPointerList<GameMakerBackground>? Backgrounds { get; set; }

    public GameMakerBgndChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Backgrounds = new GameMakerPointerList<GameMakerBackground>();
        Backgrounds.Read(
            context,
            elementReader: (ctx, notLast) => {
                var ptr = ctx.ReadInt32();

                if (context.Position % context.VersionInfo.BackgroundAlignment != 0)
                    throw new InvalidDataException($"Expected background pointer to be aligned to {context.VersionInfo.BackgroundAlignment} bytes, got {context.Position % context.VersionInfo.BackgroundAlignment}.");

                return ctx.ReadPointerAndObject<GameMakerBackground>(ptr, returnAfter: notLast);
            }
        );
    }

    public override void Write(SerializationContext context) {
        Backgrounds!.Write(
            context,
            beforeWriter: (ctx, i, count) => {
                ctx.Pad(context.VersionInfo.BackgroundAlignment);
            }
        );
    }
}
