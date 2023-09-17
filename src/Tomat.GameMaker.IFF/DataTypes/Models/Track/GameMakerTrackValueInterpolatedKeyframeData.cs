using System.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackValueInterpolatedKeyframeData : IGameMakerSerializable {
    public int Value { get; set; }

    public bool IsCurveEmbedded { get; set; }

    public IAnimationCurve? AnimationCurve { get; set; }

    public int AnimationCurveId { get; set; }

    public void Read(DeserializationContext context) {
        Value = context.ReadInt32();
        IsCurveEmbedded = context.ReadBoolean(wide: true);

        if (IsCurveEmbedded) {
            AnimationCurveId = context.ReadInt32();
            if (AnimationCurveId != -1)
                throw new InvalidDataException("Expected curve ID of -1!");

            AnimationCurve = new GameMakerAnimationCurve();
            AnimationCurve.Read(context);
        }
        else {
            AnimationCurveId = context.ReadInt32();
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Value);
        context.Write(IsCurveEmbedded, wide: true);

        if (IsCurveEmbedded) {
            context.Write(AnimationCurveId); // Expected to be -1.
            AnimationCurve!.Write(context);
        }
        else {
            context.Write(AnimationCurveId);
        }
    }
}
