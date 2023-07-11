using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Font;

public sealed class GameMakerGlyph : IGameMakerSerializable {
    public ushort Character { get; set; }

    public ushort X { get; set; }

    public ushort Y { get; set; }

    public ushort Width { get; set; }

    public ushort Height { get; set; }

    public short Shift { get; set; }

    public short Offset { get; set; }

    public List<GameMakerKerning>? Kerning { get; set; }

    public void Read(DeserializationContext context) {
        Character = context.Reader.ReadUInt16();
        X = context.Reader.ReadUInt16();
        Y = context.Reader.ReadUInt16();
        Width = context.Reader.ReadUInt16();
        Height = context.Reader.ReadUInt16();
        Shift = context.Reader.ReadInt16();
        Offset = context.Reader.ReadInt16();

        Kerning = new List<GameMakerKerning>();

        for (var i = context.Reader.ReadUInt16(); i > 0; i--) {
            var kerning = new GameMakerKerning();
            kerning.Read(context);
            Kerning.Add(kerning);
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Character);
        context.Writer.Write(X);
        context.Writer.Write(Y);
        context.Writer.Write(Width);
        context.Writer.Write(Height);
        context.Writer.Write(Shift);
        context.Writer.Write(Offset);
        
        context.Writer.Write((ushort)Kerning.Count);
        foreach (var kerning in Kerning)
            kerning.Write(context);
    }
}
