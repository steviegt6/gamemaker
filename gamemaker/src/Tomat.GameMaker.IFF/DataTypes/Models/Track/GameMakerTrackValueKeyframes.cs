using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public class GameMakerTrackValueKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerTrackValueKeyframeData>> Values { get; set; } = null!;

    public override void Read(DeserializationContext context) {
        Values = context.ReadList<GameMakerKeyframe<GameMakerTrackValueKeyframeData>>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Values);
    }
}
