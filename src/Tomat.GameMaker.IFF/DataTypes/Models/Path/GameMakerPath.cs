using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Path;

public sealed class GameMakerPath : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public bool Smooth { get; set; }

    public bool Closed { get; set; }

    public uint Precision { get; set; }

    public GameMakerList<GameMakerPathPoint> Points { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Smooth = context.ReadBoolean(wide: true);
        Closed = context.ReadBoolean(wide: true);
        Precision = context.ReadUInt32();
        Points = context.ReadList<GameMakerPathPoint>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Smooth, wide: true);
        context.Write(Closed, wide: true);
        context.Write(Precision);
        context.Write(Points);
    }
}
