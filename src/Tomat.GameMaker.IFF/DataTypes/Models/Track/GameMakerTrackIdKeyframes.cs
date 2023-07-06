using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

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
