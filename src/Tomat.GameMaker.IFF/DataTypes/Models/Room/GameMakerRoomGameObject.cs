namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomGameObject : IGameMakerSerializable {
    public int X { get; set; }

    public int Y { get; set; }

    public int ObjectId { get; set; }

    public int InstanceId { get; set; }

    public int CreationCodeId { get; set; }

    public float ScaleX { get; set; }

    public float ScaleY { get; set; }

    public int Color { get; set; }

    public float Angle { get; set; }

    // In some late 1.4 version and above.
    public int PreCreateCodeId { get; set; }

    // GMS 2.2.2.302+
    public float ImageSpeed { get; set; }

    public int ImageIndex { get; set; }

    public void Read(DeserializationContext context) {
        X = context.ReadInt32();
        Y = context.ReadInt32();
        ObjectId = context.ReadInt32();
        InstanceId = context.ReadInt32();
        CreationCodeId = context.ReadInt32();
        ScaleX = context.ReadSingle();
        ScaleY = context.ReadSingle();

        if (context.VersionInfo.IsAtLeast(GM_2_2_2_302)) {
            ImageSpeed = context.ReadSingle();
            ImageIndex = context.ReadInt32();
        }

        Color = context.ReadInt32();
        Angle = context.ReadSingle();

        if (context.VersionInfo.RoomsAndObjectsUsePreCreate)
            PreCreateCodeId = context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(X);
        context.Write(Y);
        context.Write(ObjectId);
        context.Write(InstanceId);
        context.Write(CreationCodeId);
        context.Write(ScaleX);
        context.Write(ScaleY);

        if (context.VersionInfo.IsAtLeast(GM_2_2_2_302)) {
            context.Write(ImageSpeed);
            context.Write(ImageIndex);
        }

        context.Write(Color);
        context.Write(Angle);

        if (context.VersionInfo.RoomsAndObjectsUsePreCreate)
            context.Write(PreCreateCodeId);
    }
}
