using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackValueInterpolatedKeyframes : GameMakerTrackKeyframes {
    public GameMakerTrackValueInterpolatedKeyframesInterpolationType InterpolationType { get; set; }

    public GameMakerList<GameMakerKeyframe<GameMakerTrackValueInterpolatedKeyframeData>>? Keyframes { get; set; }

    public override void Read(DeserializationContext context) {
        InterpolationType = (GameMakerTrackValueInterpolatedKeyframesInterpolationType)context.Reader.ReadInt32();

        Keyframes = new GameMakerList<GameMakerKeyframe<GameMakerTrackValueInterpolatedKeyframeData>>();
        Keyframes.Read(context);
    }

    public override void Write(SerializationContext context) {
        context.Writer.Write((int)InterpolationType);
        Keyframes!.Write(context);
    }
}
