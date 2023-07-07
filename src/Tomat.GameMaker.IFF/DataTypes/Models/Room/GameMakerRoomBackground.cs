using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomBackground : IGameMakerSerializable {
    public bool Enabled { get; set; }

    public bool Foreground { get; set; }

    public int BackgroundId { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int TileX { get; set; }

    public int TileY { get; set; }

    public int SpeedX { get; set; }

    public int SpeedY { get; set; }

    public bool Stretch { get; set; }

    public void Read(DeserializationContext context) {
        Enabled = context.Reader.ReadBoolean(wide: true);
        Foreground = context.Reader.ReadBoolean(wide: true);
        BackgroundId = context.Reader.ReadInt32();
        X = context.Reader.ReadInt32();
        Y = context.Reader.ReadInt32();
        TileX = context.Reader.ReadInt32();
        TileY = context.Reader.ReadInt32();
        SpeedX = context.Reader.ReadInt32();
        SpeedY = context.Reader.ReadInt32();
        Stretch = context.Reader.ReadBoolean(wide: true);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Enabled, wide: true);
        context.Writer.Write(Foreground, wide: true);
        context.Writer.Write(BackgroundId);
        context.Writer.Write(X);
        context.Writer.Write(Y);
        context.Writer.Write(TileX);
        context.Writer.Write(TileY);
        context.Writer.Write(SpeedX);
        context.Writer.Write(SpeedY);
    }
}
