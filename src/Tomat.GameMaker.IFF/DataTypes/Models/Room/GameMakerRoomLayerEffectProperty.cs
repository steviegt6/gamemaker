using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerEffectProperty : IGameMakerSerializable {
    public GameMakerRoomLayerEffectPropertyType Type { get; set; }

    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Value { get; set; }

    public void Read(DeserializationContext context) {
        Type = (GameMakerRoomLayerEffectPropertyType)context.Reader.ReadInt32();
        Name = context.ReadPointerAndObject<GameMakerString>();
        Value = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write((int)Type);
        context.Writer.Write(Name);
        context.Writer.Write(Value);
    }
}
