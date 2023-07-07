using Tomat.GameMaker.IFF.Chunks;

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
        X = context.Reader.ReadInt32();
        Y = context.Reader.ReadInt32();
        AssetId = context.Reader.ReadInt32();
        SourceX = context.Reader.ReadInt32();
        SourceY = context.Reader.ReadInt32();
        Width = context.Reader.ReadInt32();
        Height = context.Reader.ReadInt32();
        Depth = context.Reader.ReadInt32();
        Id = context.Reader.ReadInt32();
        ScaleX = context.Reader.ReadSingle();
        ScaleY = context.Reader.ReadSingle();
        Color = context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(X);
        context.Writer.Write(Y);
        context.Writer.Write(AssetId);
        context.Writer.Write(SourceX);
        context.Writer.Write(SourceY);
        context.Writer.Write(Width);
        context.Writer.Write(Height);
        context.Writer.Write(Depth);
        context.Writer.Write(Id);
        context.Writer.Write(ScaleX);
        context.Writer.Write(ScaleY);
        context.Writer.Write(Color);
    }
}
