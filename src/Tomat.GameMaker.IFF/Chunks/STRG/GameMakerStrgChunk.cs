using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks.Contexts;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.STRG;

public sealed class GameMakerStrgChunk : AbstractChunk {
    public const string NAME = "STRG";

    public List<GameMakerPointer<GameMakerString>>? Strings { get; set; }

    public GameMakerStrgChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Strings = context.Reader.ReadPointerList<GameMakerString>(
            context,
            null,
            null,
            (ctx, _) => {
                var addr = ctx.Reader.ReadInt32();

                if (ctx.Reader.Position % ctx.VersionInfo.StringAlignment != 0)
                    throw new System.Exception("String not aligned to expected string alignment.");

                return ctx.Reader.ReadPointerAndObject<GameMakerString>(addr, ctx, returnAfter: true, useTypeOffset: false);
            }
        );
    }

    public override void Write(SerializationContext context) {
        context.Writer.WritePointerList(
            Strings!,
            context,
            (ctx, _, _) => {
                ctx.Writer.Pad(ctx.VersionInfo.StringAlignment);
            },
            null,
            null,
            (ctx, element) => {
                ctx.Writer.Write(element, useTypeOffset: false);
            }
        );

        context.Writer.Pad(128);
    }
}
