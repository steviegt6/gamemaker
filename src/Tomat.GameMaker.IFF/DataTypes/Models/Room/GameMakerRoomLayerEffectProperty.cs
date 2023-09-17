namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerEffectProperty : IGameMakerSerializable {
    public GameMakerRoomLayerEffectPropertyType Type { get; set; }

    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Value { get; set; }

    public void Read(DeserializationContext context) {
        Type = (GameMakerRoomLayerEffectPropertyType)context.ReadInt32();
        Name = context.ReadPointerAndObject<GameMakerString>();
        Value = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Write((int)Type);
        context.Write(Name);
        context.Write(Value);
    }
}
