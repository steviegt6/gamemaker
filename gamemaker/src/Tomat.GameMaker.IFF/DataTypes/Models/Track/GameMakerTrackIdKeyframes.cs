using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackIdKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerTrackIdKeyframeData>> Ids { get; set; } = null!;

    public override void Read(DeserializationContext context) {
        Ids = context.ReadList<GameMakerKeyframe<GameMakerTrackIdKeyframeData>>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Ids);
    }
}
