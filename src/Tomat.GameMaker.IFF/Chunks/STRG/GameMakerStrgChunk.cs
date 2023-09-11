using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.STRG;

internal sealed class GameMakerStrgChunk : AbstractChunk,
                                           IStrgChunk {
    public const string NAME = "STRG";

    public GameMakerPointerList<GameMakerString> Strings { get; set; } = null!;

    public GameMakerStrgChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Strings = context.ReadPointerList<GameMakerString>(
            elementReader: (ctx, _) => {
                var addr = ctx.ReadInt32();

                if (ctx.Position % ctx.VersionInfo.StringAlignment != 0)
                    throw new System.Exception("String not aligned to expected string alignment.");

                return ctx.ReadPointerAndObject<GameMakerString>(addr, useTypeOffset: false);
            }
        );
    }

    public override void Write(SerializationContext context) {
        context.Write(
            Strings,
            beforeWriter: (ctx, _, _) => {
                ctx.Align(ctx.VersionInfo.StringAlignment);
            },
            elementPointerWriter: (ctx, element) => {
                ctx.Write(element, useTypeOffset: false);
            }
        );

        context.Align(128);
    }
}
