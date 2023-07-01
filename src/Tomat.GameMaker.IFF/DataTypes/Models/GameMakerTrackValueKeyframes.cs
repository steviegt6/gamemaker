using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public class GameMakerTrackValueKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerTrackValueKeyframeData>>? Values { get; set; }

    public override void Read(DeserializationContext context) {
        Values = new GameMakerList<GameMakerKeyframe<GameMakerTrackValueKeyframeData>>();
        Values.Read(context);
    }

    public override void Write(SerializationContext context) {
        Values!.Write(context);
    }
}
