﻿using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Audio;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.AUDO;

public sealed class GameMakerAudoChunk : AbstractChunk {
    public const string NAME = "AUDO";

    public GameMakerPointerList<GameMakerAudio> Audio { get; set; } = null!;

    public GameMakerAudoChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Audio = context.ReadPointerList<GameMakerAudio>();
    }

    public override void Write(SerializationContext context) {
        context.Write(
            Audio,
            beforeWriter: (ctx, _, _) => {
                ctx.Pad(4);
            }
        );
    }
}