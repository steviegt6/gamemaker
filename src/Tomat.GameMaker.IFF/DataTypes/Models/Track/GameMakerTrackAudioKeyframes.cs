using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackAudioKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerTrackAudioKeyframeData>> Keyframes { get; set; } = null!;

    public override void Read(DeserializationContext context) {
        Keyframes = context.ReadList<GameMakerKeyframe<GameMakerTrackAudioKeyframeData>>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Keyframes);
    }
}
