using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Object;

public sealed class GameMakerObjectEvent : IGameMakerSerializable {
    public int SubType { get; set; }

    public GameMakerPointerList<GameMakerObjectEventAction>? Actions { get; set; }

    public void Read(DeserializationContext context) {
        SubType = context.Reader.ReadInt32();

        Actions = new GameMakerPointerList<GameMakerObjectEventAction>();
        Actions.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(SubType);
        Actions!.Write(context);
    }
}
