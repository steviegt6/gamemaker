using System;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public interface ICurvePoint : IGameMakerSerializableWithComponents {
    float X { get; set; }

    float Value { get; set; }
}

public interface ICurvePointBezierComponent {
    float BezierX0 { get; set; }

    float BezierY0 { get; set; }

    float BezierX1 { get; set; }

    float BezierY1 { get; set; }
}

internal sealed class GameMakerCurvePoint : AbstractSerializableWithComponents,
                                            ICurvePoint {
    internal sealed class CurvePointBezierComponent : ICurvePointBezierComponent {
        public required float BezierX0 { get; set; }

        public required float BezierY0 { get; set; }

        public required float BezierX1 { get; set; }

        public required float BezierY1 { get; set; }
    }

    public float X { get; set; }

    public float Value { get; set; }

    public override void Read(DeserializationContext context) {
        X = context.ReadSingle();
        Value = context.ReadSingle();

        while (true) {
            if (context.VersionInfo.IsAtLeast(GM_2_3_1)) {
                var bezierX0 = context.ReadSingle();
                var bezierY0 = context.ReadSingle();
                var bezierX1 = context.ReadSingle();
                var bezierY1 = context.ReadSingle();
                AddComponent<ICurvePointBezierComponent>(
                    new CurvePointBezierComponent {
                        BezierX0 = bezierX0,
                        BezierY0 = bezierY0,
                        BezierX1 = bezierX1,
                        BezierY1 = bezierY1
                    }
                );
                break;
            }

            // Bitwise AND here is important because we want both to run.
            if (context.ReadInt32() != 0 & context.ReadInt32() != 0) {
                context.VersionInfo.UpdateTo(GM_2_3_1);
                context.Position -= sizeof(int) * 2;
                continue;
            }

            break;
        }
    }

    public override void Write(SerializationContext context) {
        context.Write(X);
        context.Write(Value);

        if (context.VersionInfo.IsAtLeast(GM_2_3_1)) {
            if (!TryGetComponent<ICurvePointBezierComponent>(out var bezierComponent))
                throw new InvalidOperationException("2.3.1+ curve point did not contain bezier data!");

            context.Write(bezierComponent.BezierX0);
            context.Write(bezierComponent.BezierY0);
            context.Write(bezierComponent.BezierX1);
            context.Write(bezierComponent.BezierY1);
        }
        else {
            context.Pad(sizeof(int) * 2);
        }
    }
}
