using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerTrackStringKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerString>>? Strings { get; set; }

    public override void Read(DeserializationContext context) {
        Strings = new GameMakerList<GameMakerKeyframe<GameMakerString>>();
        Strings.Read(context);
    }

    public override void Write(SerializationContext context) {
        Strings!.Write(context);
    }
}
