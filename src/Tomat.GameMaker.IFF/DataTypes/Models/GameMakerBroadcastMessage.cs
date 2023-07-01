using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerBroadcastMessage : IGameMakerSerializable {
    public List<GameMakerPointer<GameMakerString>>? Messages { get; set; }

    public void Read(DeserializationContext context) {
        Messages = new List<GameMakerPointer<GameMakerString>>();
        var count = context.Reader.ReadInt32();

        for (var i = 0; i < count; i++)
            Messages.Add(context.ReadPointerAndObject<GameMakerString>());
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Messages!.Count);

        foreach (var message in Messages)
            context.Writer.Write(message);
    }
}
