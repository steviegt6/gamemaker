using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

// model GMCurveChannel {
//     string* name;
//     int32 function;
//     int32 iterations;
//     int32 pointsCount;
//     GMCurvePoint[] points;
// }

public interface IAnimationCurveChannel : IGameMakerSerializable {
    GameMakerPointer<IString> Name { get; set; }

    AnimationCurveType FunctionType { get; set; }

    int Iterations { get; set; }

    int PointsCount { get; }

    List<ICurvePoint> Points { get; set; }
}

internal sealed class GameMakerAnimationCurveChannel : IAnimationCurveChannel {
    public GameMakerPointer<IString> Name { get; set; }

    public AnimationCurveType FunctionType { get; set; }

    public int Iterations { get; set; }

    public int PointsCount => Points.Count;

    public List<ICurvePoint> Points { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<IString, GameMakerString>();
        FunctionType = (AnimationCurveType)context.ReadInt32();
        Iterations = context.ReadInt32();
        Points = context.ReadList<ICurvePoint, GameMakerCurvePoint>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write((int)FunctionType);
        context.Write(Iterations);
        context.WriteList(Points);
    }
}
