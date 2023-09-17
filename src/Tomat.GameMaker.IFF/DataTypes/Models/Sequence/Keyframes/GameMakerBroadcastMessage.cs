using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

public sealed class GameMakerBroadcastMessage : IGameMakerSerializable {
    public List<GameMakerPointer<GameMakerString>>? Messages { get; set; }

    public void Read(DeserializationContext context) {
        Messages = new List<GameMakerPointer<GameMakerString>>();
        var count = context.ReadInt32();

        for (var i = 0; i < count; i++)
            Messages.Add(context.ReadPointerAndObject<GameMakerString>());
    }

    public void Write(SerializationContext context) {
        context.Write(Messages!.Count);

        foreach (var message in Messages)
            context.Write(message);
    }
}
