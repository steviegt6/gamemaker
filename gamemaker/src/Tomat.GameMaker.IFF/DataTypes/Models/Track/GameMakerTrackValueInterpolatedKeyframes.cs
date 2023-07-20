using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackValueInterpolatedKeyframes : GameMakerTrackKeyframes {
    public GameMakerTrackValueInterpolatedKeyframesInterpolationType InterpolationType { get; set; }

    public GameMakerList<GameMakerKeyframe<GameMakerTrackValueInterpolatedKeyframeData>> Keyframes { get; set; } = null!;

    public override void Read(DeserializationContext context) {
        InterpolationType = (GameMakerTrackValueInterpolatedKeyframesInterpolationType)context.ReadInt32();

        Keyframes = context.ReadList<GameMakerKeyframe<GameMakerTrackValueInterpolatedKeyframeData>>();
    }

    public override void Write(SerializationContext context) {
        context.Write((int)InterpolationType);
        context.Write(Keyframes);
    }
}
