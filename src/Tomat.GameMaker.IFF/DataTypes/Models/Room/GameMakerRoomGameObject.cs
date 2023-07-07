using Tomat.GameMaker.IFF.Chunks;

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
        X = context.Reader.ReadInt32();
        Y = context.Reader.ReadInt32();
        ObjectId = context.Reader.ReadInt32();
        InstanceId = context.Reader.ReadInt32();
        CreationCodeId = context.Reader.ReadInt32();
        ScaleX = context.Reader.ReadSingle();
        ScaleY = context.Reader.ReadSingle();

        if (context.VersionInfo.IsAtLeast(GM_2_2_2_302)) {
            ImageSpeed = context.Reader.ReadSingle();
            ImageIndex = context.Reader.ReadInt32();
        }

        Color = context.Reader.ReadInt32();
        Angle = context.Reader.ReadSingle();

        if (context.VersionInfo.RoomsAndObjectsUsePreCreate)
            PreCreateCodeId = context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(X);
        context.Writer.Write(Y);
        context.Writer.Write(ObjectId);
        context.Writer.Write(InstanceId);
        context.Writer.Write(CreationCodeId);
        context.Writer.Write(ScaleX);
        context.Writer.Write(ScaleY);

        if (context.VersionInfo.IsAtLeast(GM_2_2_2_302)) {
            context.Writer.Write(ImageSpeed);
            context.Writer.Write(ImageIndex);
        }

        context.Writer.Write(Color);
        context.Writer.Write(Angle);

        if (context.VersionInfo.RoomsAndObjectsUsePreCreate)
            context.Writer.Write(PreCreateCodeId);
    }
}
