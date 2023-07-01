﻿using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.STRG;

public sealed class GameMakerStrgChunk : AbstractChunk {
    public const string NAME = "STRG";

    public GameMakerPointerList<GameMakerString>? Strings { get; set; }

    public GameMakerStrgChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Strings = new GameMakerPointerList<GameMakerString>();
        Strings.Read(
            context,
            elementReader: (ctx, _) => {
                var addr = ctx.Reader.ReadInt32();

                if (ctx.Reader.Position % ctx.VersionInfo.StringAlignment != 0)
                    throw new System.Exception("String not aligned to expected string alignment.");

                return ctx.ReadPointerAndObject<GameMakerString>(addr, useTypeOffset: false);
            }
        );
    }

    public override void Write(SerializationContext context) {
        Strings!.Write(
            context,
            beforeWriter: (ctx, _, _) => {
                ctx.Writer.Pad(ctx.VersionInfo.StringAlignment);
            },
            elementPointerWriter: (ctx, element) => {
                ctx.Writer.Write(element, useTypeOffset: false);
            }
        );

        context.Writer.Pad(128);
    }
}
