using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

public sealed class GameMakerAnimationCurveChannel : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerAnimationCurveChannelFunctionType FunctionType { get; set; }

    public ushort Iterations { get; set; }

    public GameMakerList<GameMakerAnimationCurveChannelPoint> Points { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        FunctionType = (GameMakerAnimationCurveChannelFunctionType)context.ReadInt32();
        Iterations = (ushort)context.ReadUInt32(); // TODO: Uhh... okay?

        Points = context.ReadList<GameMakerAnimationCurveChannelPoint>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write((int)FunctionType);
        context.Write((uint)Iterations); // TODO: Uhh... okay?
        context.Write(Points);
    }
}
