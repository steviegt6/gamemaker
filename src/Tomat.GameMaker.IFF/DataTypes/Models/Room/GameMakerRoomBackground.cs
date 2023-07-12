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
        Enabled = context.ReadBoolean(wide: true);
        Foreground = context.ReadBoolean(wide: true);
        BackgroundId = context.ReadInt32();
        X = context.ReadInt32();
        Y = context.ReadInt32();
        TileX = context.ReadInt32();
        TileY = context.ReadInt32();
        SpeedX = context.ReadInt32();
        SpeedY = context.ReadInt32();
        Stretch = context.ReadBoolean(wide: true);
    }

    public void Write(SerializationContext context) {
        context.Write(Enabled, wide: true);
        context.Write(Foreground, wide: true);
        context.Write(BackgroundId);
        context.Write(X);
        context.Write(Y);
        context.Write(TileX);
        context.Write(TileY);
        context.Write(SpeedX);
        context.Write(SpeedY);
        context.Write(Stretch, wide: true);
    }
}
