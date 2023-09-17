namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomTile : IGameMakerSerializable {
    public int X { get; set; }

    public int Y { get; set; }

    // Sprite in GMS2, background before
    public int AssetId { get; set; }

    public int SourceX { get; set; }

    public int SourceY { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Depth { get; set; }

    public int Id { get; set; }

    public float ScaleX { get; set; }

    public float ScaleY { get; set; }

    public int Color { get; set; }

    public void Read(DeserializationContext context) {
        X = context.ReadInt32();
        Y = context.ReadInt32();
        AssetId = context.ReadInt32();
        SourceX = context.ReadInt32();
        SourceY = context.ReadInt32();
        Width = context.ReadInt32();
        Height = context.ReadInt32();
        Depth = context.ReadInt32();
        Id = context.ReadInt32();
        ScaleX = context.ReadSingle();
        ScaleY = context.ReadSingle();
        Color = context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(X);
        context.Write(Y);
        context.Write(AssetId);
        context.Write(SourceX);
        context.Write(SourceY);
        context.Write(Width);
        context.Write(Height);
        context.Write(Depth);
        context.Write(Id);
        context.Write(ScaleX);
        context.Write(ScaleY);
        context.Write(Color);
    }
}
