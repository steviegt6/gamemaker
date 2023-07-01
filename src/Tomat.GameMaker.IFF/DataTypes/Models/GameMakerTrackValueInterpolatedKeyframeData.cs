using System.IO;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerTrackValueInterpolatedKeyframeData : IGameMakerSerializable {
    public int Value { get; set; }

    public bool IsCurveEmbedded { get; set; }

    public GameMakerAnimationCurve? AnimationCurve { get; set; }

    public int AnimationCurveId { get; set; }

    public void Read(DeserializationContext context) {
        Value = context.Reader.ReadInt32();
        IsCurveEmbedded = context.Reader.ReadBoolean(wide: true);

        if (IsCurveEmbedded) {
            AnimationCurveId = context.Reader.ReadInt32();
            if (AnimationCurveId != -1)
                throw new InvalidDataException("Expected curve ID of -1!");

            AnimationCurve = new GameMakerAnimationCurve();
            AnimationCurve.Read(context);
        }
        else {
            AnimationCurveId = context.Reader.ReadInt32();
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Value);
        context.Writer.Write(IsCurveEmbedded, wide: true);

        if (IsCurveEmbedded) {
            context.Writer.Write(AnimationCurveId); // Expected to be -1.
            AnimationCurve!.Write(context);
        }
        else {
            context.Writer.Write(AnimationCurveId);
        }
    }
}
