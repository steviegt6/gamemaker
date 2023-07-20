using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Object;

public sealed class GameMakerObjectEvent : IGameMakerSerializable {
    public int SubType { get; set; }

    public GameMakerPointerList<GameMakerObjectEventAction> Actions { get; set; } = null!;

    public void Read(DeserializationContext context) {
        SubType = context.ReadInt32();

        Actions = context.ReadPointerList<GameMakerObjectEventAction>();
    }

    public void Write(SerializationContext context) {
        context.Write(SubType);
        context.Write(Actions);
    }
}
