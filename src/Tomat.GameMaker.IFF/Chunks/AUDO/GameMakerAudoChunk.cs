using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks.Contexts;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.AUDO;

public sealed class GameMakerAudoChunk : AbstractChunk {
    public const string NAME = "AUDO";

    public List<GameMakerPointer<GameMakerAudio>>? Audio { get; set; }

    public GameMakerAudoChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Audio = context.Reader.ReadPointerList<GameMakerAudio>(context, null, null, null);
    }

    public override void Write(SerializationContext context) {
        context.Writer.WritePointerList(
            Audio!,
            context,
            (ctx, _, _) => {
                ctx.Writer.Pad(4);
            },
            null,
            null,
            null
        );
    }
}
