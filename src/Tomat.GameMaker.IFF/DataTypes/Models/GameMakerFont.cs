using System;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerFont : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> DisplayName { get; set; }

    public int Size { get; set; }

    // In 2.3(?), Size is a float.
    public float SizeFloat { get; set; }

    public bool Bold { get; set; }

    public bool Italic { get; set; }

    public ushort RangeStart { get; set; }

    public byte Charset { get; set; }

    public byte AntiAlias { get; set; }

    public int RangeEnd { get; set; }

    public GameMakerPointer<GameMakerTextureItem> TextureItem { get; set; }

    public float ScaleX { get; set; }

    public float ScaleY { get; set; }

    public int AscenderOffset { get; set; }

    public int Ascender { get; set; }

    public GameMakerPointerList<GameMakerGlyph>? Glyphs { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        DisplayName = context.ReadPointerAndObject<GameMakerString>();
        Size = context.Reader.ReadInt32();

        if (Size < 0) {
            context.Reader.Position -= sizeof(int);
            SizeFloat = -context.Reader.ReadSingle();
        }

        Bold = context.Reader.ReadBoolean(wide: true);
        Italic = context.Reader.ReadBoolean(wide: true);
        RangeStart = context.Reader.ReadUInt16();
        Charset = context.Reader.ReadByte();
        AntiAlias = context.Reader.ReadByte();
        RangeEnd = context.Reader.ReadInt32();
        TextureItem = context.ReadPointerAndObject<GameMakerTextureItem>();
        ScaleX = context.Reader.ReadSingle();
        ScaleY = context.Reader.ReadSingle();
        if (context.VersionInfo.FormatId >= 17)
            AscenderOffset = context.Reader.ReadInt32();
        if (context.VersionInfo.IsAtLeast(GM_2022_2))
            Ascender = context.Reader.ReadInt32();

        Glyphs = new GameMakerPointerList<GameMakerGlyph>();
        Glyphs.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(DisplayName);
        if (Size < 0)
            Size = BitConverter.ToInt32(BitConverter.GetBytes(-SizeFloat));
        context.Writer.Write(Size);
        context.Writer.Write(Bold, wide: true);
        context.Writer.Write(Italic, wide: true);
        context.Writer.Write(RangeStart);
        context.Writer.Write(Charset);
        context.Writer.Write(AntiAlias);
        context.Writer.Write(RangeEnd);
        context.Writer.Write(TextureItem);
        context.Writer.Write(ScaleX);
        context.Writer.Write(ScaleY);
        if (context.VersionInfo.FormatId >= 17)
            context.Writer.Write(AscenderOffset);
        if (context.VersionInfo.IsAtLeast(GM_2022_2))
            context.Writer.Write(Ascender);
        Glyphs!.Write(context);
    }
}
