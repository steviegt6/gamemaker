using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerTexturePage : IGameMakerSerializable {
    public uint Scaled { get; set; }

    public uint GeneratedMips { get; set; }

    public GameMakerPointer<GameMakerTextureData> TextureData { get; set; }

    // 2022.9+
    public int TextureWidth { get; set; }

    public int TextureHeight { get; set; }

    public int IndexInGroup { get; set; }

    public void Read(DeserializationContext context) {
        Scaled = context.Reader.ReadUInt32();

        if (context.VersionInfo.IsAtLeast(GM_2))
            GeneratedMips = context.Reader.ReadUInt32();

        if (context.VersionInfo.IsAtLeast(GM_2022_3))
            context.Reader.Position += sizeof(int); // Ignore data length.

        if (context.VersionInfo.IsAtLeast(GM_2022_9)) {
            TextureWidth = context.Reader.ReadInt32();
            TextureHeight = context.Reader.ReadInt32();
            IndexInGroup = context.Reader.ReadInt32();
        }

        TextureData = context.ReadPointerAndObject<GameMakerTextureData>();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Scaled);

        if (context.VersionInfo.IsAtLeast(GM_2))
            context.Writer.Write(GeneratedMips);

        if (context.VersionInfo.IsAtLeast(GM_2022_3)) {
            TextureData.ExpectObject().LengthOffset = context.Writer.Position;
            context.Writer.Write(0); // Skip data length.
        }

        if (context.VersionInfo.IsAtLeast(GM_2022_9)) {
            context.Writer.Write(TextureWidth);
            context.Writer.Write(TextureHeight);
            context.Writer.Write(IndexInGroup);
        }

        context.Writer.Write(TextureData);
    }
}
