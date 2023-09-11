using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

/* model GMCurveChannel {
 *     string* name;
 *     int32 function;
 *     int32 iterations;
 *     int32 pointsCount;
 *     GMCurvePoint[] points;
 * }
 */

public sealed class GameMakerAnimationCurveChannel : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerAnimationCurveChannelFunctionType FunctionType { get; set; }

    public int Iterations { get; set; }

    public GameMakerList<GameMakerCurvePoint> Points { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        FunctionType = (GameMakerAnimationCurveChannelFunctionType)context.ReadInt32();
        Iterations = context.ReadInt32();
        Points = context.ReadList<GameMakerCurvePoint>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write((int)FunctionType);
        context.Write(Iterations);
        context.Write(Points);
    }
}
