using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerEffect : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> EffectType { get; set; }

    public GameMakerList<GameMakerRoomLayerEffectProperty> Properties { get; set; } = null!;

    public void Read(DeserializationContext context) {
        if (context.VersionInfo.IsAtLeast(GM_2022_1))
            return;

        EffectType = context.ReadPointerAndObject<GameMakerString>();
        Properties = context.ReadList<GameMakerRoomLayerEffectProperty>();
    }

    public void Write(SerializationContext context) {
        if (context.VersionInfo.IsAtLeast(GM_2022_1))
            return;

        context.Write(EffectType);
        context.Write(Properties);
    }
}
