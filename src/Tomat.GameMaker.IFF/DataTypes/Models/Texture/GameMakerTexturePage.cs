namespace Tomat.GameMaker.IFF.DataTypes.Models.Texture;

public sealed class GameMakerTexturePage : IGameMakerSerializable {
    public uint Scaled { get; set; }

    public uint GeneratedMips { get; set; }

    public GameMakerPointer<GameMakerTextureData> TextureData { get; set; }

    // 2022.9+
    public int TextureWidth { get; set; }

    public int TextureHeight { get; set; }

    public int IndexInGroup { get; set; }

    public void Read(DeserializationContext context) {
        Scaled = context.ReadUInt32();

        if (context.VersionInfo.IsAtLeast(GM_2))
            GeneratedMips = context.ReadUInt32();

        if (context.VersionInfo.IsAtLeast(GM_2022_3))
            context.Position += sizeof(int); // Ignore data length.

        if (context.VersionInfo.IsAtLeast(GM_2022_9)) {
            TextureWidth = context.ReadInt32();
            TextureHeight = context.ReadInt32();
            IndexInGroup = context.ReadInt32();
        }

        TextureData = context.ReadPointerAndObject<GameMakerTextureData>();
    }

    public void Write(SerializationContext context) {
        context.Write(Scaled);

        if (context.VersionInfo.IsAtLeast(GM_2))
            context.Write(GeneratedMips);

        if (context.VersionInfo.IsAtLeast(GM_2022_3)) {
            TextureData.ExpectObject().LengthOffset = context.Position;
            context.Write(0); // Skip data length.
        }

        if (context.VersionInfo.IsAtLeast(GM_2022_9)) {
            context.Write(TextureWidth);
            context.Write(TextureHeight);
            context.Write(IndexInGroup);
        }

        context.Write(TextureData);
    }
}
