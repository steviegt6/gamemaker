using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

// TODO: Documentation.
public sealed class GameMakerTextureItem : IGameMakerSerializable {
    public ushort SourceX { get; set; }

    public ushort SourceY { get; set; }

    public ushort SourceWidth { get; set; }

    public ushort SourceHeight { get; set; }

    public ushort TargetX { get; set; }

    public ushort TargetY { get; set; }

    public ushort TargetWidth { get; set; }

    public ushort TargetHeight { get; set; }

    public ushort BoundWidth { get; set; }

    public ushort BoundHeight { get; set; }

    // TODO: -1 is an impossible value. Handle it.
    public short TexturePageId { get; set; } = -1;

    public void Read(DeserializationContext context) {
        SourceX = context.Reader.ReadUInt16();
        SourceY = context.Reader.ReadUInt16();
        SourceWidth = context.Reader.ReadUInt16();
        SourceHeight = context.Reader.ReadUInt16();
        TargetX = context.Reader.ReadUInt16();
        TargetY = context.Reader.ReadUInt16();
        TargetWidth = context.Reader.ReadUInt16();
        TargetHeight = context.Reader.ReadUInt16();
        BoundWidth = context.Reader.ReadUInt16();
        BoundHeight = context.Reader.ReadUInt16();
        TexturePageId = context.Reader.ReadInt16();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(SourceX);
        context.Writer.Write(SourceY);
        context.Writer.Write(SourceWidth);
        context.Writer.Write(SourceHeight);
        context.Writer.Write(TargetX);
        context.Writer.Write(TargetY);
        context.Writer.Write(TargetWidth);
        context.Writer.Write(TargetHeight);
        context.Writer.Write(BoundWidth);
        context.Writer.Write(BoundHeight);
        context.Writer.Write(TexturePageId);
    }
}
