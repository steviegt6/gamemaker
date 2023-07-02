using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerChannel : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerChannelFunctionType FunctionType { get; set; }

    public ushort Iterations { get; set; }

    public GameMakerList<GameMakerPoint>? Points { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        FunctionType = (GameMakerChannelFunctionType)context.Reader.ReadInt32();
        Iterations = (ushort)context.Reader.ReadUInt32(); // TODO: Uhh... okay?

        Points = new GameMakerList<GameMakerPoint>();
        Points.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write((int)FunctionType);
        context.Writer.Write(Iterations);
        Points!.Write(context);
    }
}
