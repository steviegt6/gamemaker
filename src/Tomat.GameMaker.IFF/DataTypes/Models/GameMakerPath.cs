using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerPath : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public bool Smooth { get; set; }

    public bool Closed { get; set; }

    public uint Precision { get; set; }

    public GameMakerList<GameMakerPathPoint>? Points { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Smooth = context.Reader.ReadBoolean(wide: true);
        Closed = context.Reader.ReadBoolean(wide: true);
        Precision = context.Reader.ReadUInt32();
        Points = new GameMakerList<GameMakerPathPoint>();
        Points.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Smooth, wide: true);
        context.Writer.Write(Closed, wide: true);
        context.Writer.Write(Precision);
        Points?.Write(context);
    }
}
