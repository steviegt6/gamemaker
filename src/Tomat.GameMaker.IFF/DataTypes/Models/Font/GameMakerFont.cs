using System;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Font;

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

    public GameMakerPointerList<GameMakerGlyph> Glyphs { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        DisplayName = context.ReadPointerAndObject<GameMakerString>();
        Size = context.ReadInt32();

        if (Size < 0) {
            context.Position -= sizeof(int);
            SizeFloat = -context.ReadSingle();
        }

        Bold = context.ReadBoolean(wide: true);
        Italic = context.ReadBoolean(wide: true);
        RangeStart = context.ReadUInt16();
        Charset = context.ReadByte();
        AntiAlias = context.ReadByte();
        RangeEnd = context.ReadInt32();
        TextureItem = context.ReadPointerAndObject<GameMakerTextureItem>();
        ScaleX = context.ReadSingle();
        ScaleY = context.ReadSingle();
        if (context.VersionInfo.FormatId >= 17)
            AscenderOffset = context.ReadInt32();
        if (context.VersionInfo.IsAtLeast(GM_2022_2))
            Ascender = context.ReadInt32();

        Glyphs = context.ReadPointerList<GameMakerGlyph>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(DisplayName);
        if (Size < 0)
            Size = BitConverter.ToInt32(BitConverter.GetBytes(-SizeFloat));
        context.Write(Size);
        context.Write(Bold, wide: true);
        context.Write(Italic, wide: true);
        context.Write(RangeStart);
        context.Write(Charset);
        context.Write(AntiAlias);
        context.Write(RangeEnd);
        context.Write(TextureItem);
        context.Write(ScaleX);
        context.Write(ScaleY);
        if (context.VersionInfo.FormatId >= 17)
            context.Write(AscenderOffset);
        if (context.VersionInfo.IsAtLeast(GM_2022_2))
            context.Write(Ascender);
        context.Write(Glyphs);
    }
}
