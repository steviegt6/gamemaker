using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerTrackIdKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerTrackIdKeyframeData>>? Ids { get; set; }

    public override void Read(DeserializationContext context) {
        Ids = new GameMakerList<GameMakerKeyframe<GameMakerTrackIdKeyframeData>>();
        Ids.Read(context);
    }

    public override void Write(SerializationContext context) {
        Ids!.Write(context);
    }
}
