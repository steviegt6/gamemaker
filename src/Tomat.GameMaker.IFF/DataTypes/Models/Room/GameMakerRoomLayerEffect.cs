using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerEffect : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> EffectType { get; set; }

    public GameMakerList<GameMakerRoomLayerEffectProperty> Properties { get; set; } = null!;

    public void Read(DeserializationContext context) {
        EffectType = context.ReadPointerAndObject<GameMakerString>();
        Properties = context.ReadList<GameMakerRoomLayerEffectProperty>();
    }

    public void Write(SerializationContext context) {
        context.Write(EffectType);
        context.Write(Properties);
    }
}
