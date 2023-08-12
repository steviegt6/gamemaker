using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Texture;

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
        SourceX = context.ReadUInt16();
        SourceY = context.ReadUInt16();
        SourceWidth = context.ReadUInt16();
        SourceHeight = context.ReadUInt16();
        TargetX = context.ReadUInt16();
        TargetY = context.ReadUInt16();
        TargetWidth = context.ReadUInt16();
        TargetHeight = context.ReadUInt16();
        BoundWidth = context.ReadUInt16();
        BoundHeight = context.ReadUInt16();
        TexturePageId = context.ReadInt16();
    }

    public void Write(SerializationContext context) {
        context.Write(SourceX);
        context.Write(SourceY);
        context.Write(SourceWidth);
        context.Write(SourceHeight);
        context.Write(TargetX);
        context.Write(TargetY);
        context.Write(TargetWidth);
        context.Write(TargetHeight);
        context.Write(BoundWidth);
        context.Write(BoundHeight);
        context.Write(TexturePageId);
    }
}
