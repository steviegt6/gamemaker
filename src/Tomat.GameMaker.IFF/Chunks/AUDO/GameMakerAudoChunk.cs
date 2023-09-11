using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Audio;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.AUDO;

internal sealed class GameMakerAudoChunk : AbstractChunk,
                                           IAudoChunk {
    public const string NAME = "AUDO";

    public List<GameMakerPointer<IAudio>> Audio { get; set; } = null!;

    public GameMakerAudoChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Audio = context.ReadPointerList<IAudio, GameMakerAudio>(beforeRead: (ctx, _, _) => ctx.GmAlign(3));
    }

    public override void Write(SerializationContext context) {
        context.WritePointerList(Audio, beforeWrite: (ctx, _, _) => ctx.GmAlign(3));
    }
}
