using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

/* model GMCurvePoint {
 *     float x;
 *     float value;
 * #if VERSION >= 2.3.1
 *     float bezierX0;
 *     float bezierY0;
 *     float bezierX1;
 *     float bezierY1;
 * #else
 *     pad[8];
 * #endif
 * }
 */

public sealed class GameMakerCurvePoint : IGameMakerSerializable {
    public float X { get; set; }

    public float Value { get; set; }

    public float BezierX0 { get; set; }

    public float BezierY0 { get; set; }

    public float BezierX1 { get; set; }

    public float BezierY1 { get; set; }

    public void Read(DeserializationContext context) {
        X = context.ReadSingle();
        Value = context.ReadSingle();

        // Bitwise AND here is important because we want both to run.
        if (context.ReadInt32() == 0 & context.ReadInt32() == 0)
            context.VersionInfo.UpdateTo(GM_2_3_1);
        else
            context.Position -= sizeof(int) * 2;

        if (context.VersionInfo.IsAtLeast(GM_2_3_1)) {
            BezierX0 = context.ReadSingle();
            BezierY0 = context.ReadSingle();
            BezierX1 = context.ReadSingle();
            BezierY1 = context.ReadSingle();
        }
        else {
            // Skip over the aforementioned should-be-zero int32 values on older
            // versions.
            context.Pad(sizeof(int) * 2);
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
            context.Pad(sizeof(int) * 2);
        }
    }
}
