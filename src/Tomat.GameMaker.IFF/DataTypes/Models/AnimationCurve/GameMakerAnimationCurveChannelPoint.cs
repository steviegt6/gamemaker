using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

public sealed class GameMakerAnimationCurveChannelPoint : IGameMakerSerializable {
    public float X { get; set; }

    public float Value { get; set; }

    public float BezierX0 { get; set; }

    public float BezierY0 { get; set; }

    public float BezierX1 { get; set; }

    public float BezierY1 { get; set; }

    public void Read(DeserializationContext context) {
        X = context.ReadSingle();
        Value = context.ReadSingle();

        // In 2.3, an int32 with a value of zero would be set here. It cannot be
        // version 2.3 if this value isn't zero.
        if (context.ReadUInt32() != 0) {
            context.VersionInfo.UpdateTo(GM_2_3_1);
            context.Position -= sizeof(uint);
        }
        else {
            // If BezierX0 equals zero (above), then BezierY0 equals zero as
            // well.
            if (context.ReadUInt32() == 0)
                context.VersionInfo.UpdateTo(GM_2_3_1);
            context.Position -= sizeof(uint) * 2;
        }

        if (context.VersionInfo.IsAtLeast(GM_2_3_1)) {
            BezierX0 = context.ReadSingle();
            BezierY0 = context.ReadSingle();
            BezierX1 = context.ReadSingle();
            BezierY1 = context.ReadSingle();
        }
        else {
            // Skip over the aforementioned should-be-zero int32 values on older
            // versions.
            context.Position += sizeof(uint);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(X);
        context.Write(Value);

        if (context.VersionInfo.IsAtLeast(GM_2_3_1)) {
            context.Write(BezierX0);
            context.Write(BezierY0);
            context.Write(BezierX1);
            context.Write(BezierY1);
        }
        else {
            // Write out the aforementioned should-be-zero int32 values on older
            // versions.
            context.Write(0);
            context.Write(0);
        }
    }
}
