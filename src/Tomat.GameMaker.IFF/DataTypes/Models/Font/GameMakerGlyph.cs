using System.Collections.Generic;

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
        Character = context.ReadUInt16();
        X = context.ReadUInt16();
        Y = context.ReadUInt16();
        Width = context.ReadUInt16();
        Height = context.ReadUInt16();
        Shift = context.ReadInt16();
        Offset = context.ReadInt16();

        Kerning = new List<GameMakerKerning>();

        for (var i = context.ReadUInt16(); i > 0; i--) {
            var kerning = new GameMakerKerning();
            kerning.Read(context);
            Kerning.Add(kerning);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Character);
        context.Write(X);
        context.Write(Y);
        context.Write(Width);
        context.Write(Height);
        context.Write(Shift);
        context.Write(Offset);
        
        context.Write((ushort)Kerning!.Count);
        foreach (var kerning in Kerning)
            kerning.Write(context);
    }
}
