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
        Enabled = context.ReadBoolean(wide: true);
        ViewX = context.ReadInt32();
        ViewY = context.ReadInt32();
        ViewWidth = context.ReadInt32();
        ViewHeight = context.ReadInt32();
        PortX = context.ReadInt32();
        PortY = context.ReadInt32();
        PortWidth = context.ReadInt32();
        PortHeight = context.ReadInt32();
        BorderX = context.ReadInt32();
        BorderY = context.ReadInt32();
        SpeedX = context.ReadInt32();
        SpeedY = context.ReadInt32();
        FollowObjectId = context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(Enabled, wide: true);
        context.Write(ViewX);
        context.Write(ViewY);
        context.Write(ViewWidth);
        context.Write(ViewHeight);
        context.Write(PortX);
        context.Write(PortY);
        context.Write(PortWidth);
        context.Write(PortHeight);
        context.Write(BorderX);
        context.Write(BorderY);
        context.Write(SpeedX);
        context.Write(SpeedY);
        context.Write(FollowObjectId);
    }
}
