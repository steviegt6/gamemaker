using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

public sealed class GameMakerMoment : IGameMakerSerializable {
    // Should be zero if none, one if there's a message.
    public int InternalCount { get; set; }

    public GameMakerPointer<GameMakerString> Event { get; set; }

    public void Read(DeserializationContext context) {
        InternalCount = context.ReadInt32();
        if (InternalCount > 0)
            Event = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Write(InternalCount);
        if (InternalCount > 0)
            context.Write(Event);
    }
}
