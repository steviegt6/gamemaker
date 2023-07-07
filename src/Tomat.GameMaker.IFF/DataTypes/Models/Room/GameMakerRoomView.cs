using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomView : IGameMakerSerializable {
    public bool Enabled { get; set; }

    public int ViewX { get; set; }

    public int ViewY { get; set; }

    public int ViewWidth { get; set; }

    public int ViewHeight { get; set; }

    public int PortX { get; set; }

    public int PortY { get; set; }

    public int PortWidth { get; set; }

    public int PortHeight { get; set; }

    public int BorderX { get; set; }

    public int BorderY { get; set; }

    public int SpeedX { get; set; }

    public int SpeedY { get; set; }

    public int FollowObjectId { get; set; }

    public void Read(DeserializationContext context) {
        Enabled = context.Reader.ReadBoolean(wide: true);
        ViewX = context.Reader.ReadInt32();
        ViewY = context.Reader.ReadInt32();
        ViewWidth = context.Reader.ReadInt32();
        ViewHeight = context.Reader.ReadInt32();
        PortX = context.Reader.ReadInt32();
        PortY = context.Reader.ReadInt32();
        PortWidth = context.Reader.ReadInt32();
        PortHeight = context.Reader.ReadInt32();
        BorderX = context.Reader.ReadInt32();
        BorderY = context.Reader.ReadInt32();
        SpeedX = context.Reader.ReadInt32();
        SpeedY = context.Reader.ReadInt32();
        FollowObjectId = context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Enabled, wide: true);
        context.Writer.Write(ViewX);
        context.Writer.Write(ViewY);
        context.Writer.Write(ViewWidth);
        context.Writer.Write(ViewHeight);
        context.Writer.Write(PortX);
        context.Writer.Write(PortY);
        context.Writer.Write(PortWidth);
        context.Writer.Write(PortHeight);
        context.Writer.Write(BorderX);
        context.Writer.Write(BorderY);
        context.Writer.Write(SpeedX);
        context.Writer.Write(SpeedY);
        context.Writer.Write(FollowObjectId);
    }
}
