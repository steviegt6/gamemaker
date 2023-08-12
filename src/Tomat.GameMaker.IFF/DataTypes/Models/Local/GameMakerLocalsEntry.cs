using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Local;

public sealed class GameMakerLocalsEntry : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public List<GameMakerLocal>? Entries { get; set; }

    public void Read(DeserializationContext context) {
        Entries = new List<GameMakerLocal>();

        var count = context.ReadInt32();
        Name = context.ReadPointerAndObject<GameMakerString>();

        for (var i = 0; i < count; i++) {
            var local = new GameMakerLocal();
            local.Read(context);
            Entries.Add(local);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Entries!.Count);
        context.Write(Name);
        foreach (var entry in Entries)
            entry.Write(context);
    }
}
