using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackAudioKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerTrackAudioKeyframeData>>? Keyframes { get; set; }

    public override void Read(DeserializationContext context) {
        Keyframes = new GameMakerList<GameMakerKeyframe<GameMakerTrackAudioKeyframeData>>();
        Keyframes.Read(context);
    }

    public override void Write(SerializationContext context) {
        Keyframes!.Write(context);
    }
}
