using System;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.TextureGroupInfo;

public sealed class GameMakerTextureGroupInfo : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Directory { get; set; }

    public GameMakerPointer<GameMakerString> Extension { get; set; }

    public GameMakerTextureGroupInfoLoadType LoadType { get; set; }

    public GameMakerPointer<GameMakerList<GameMakerTextureGroupInfoResourceId>> TexturePageIds { get; set; }

    public GameMakerPointer<GameMakerList<GameMakerTextureGroupInfoResourceId>> SpriteIds { get; set; }

    public GameMakerPointer<GameMakerList<GameMakerTextureGroupInfoResourceId>>? SpineSpriteIds { get; set; }

    public GameMakerPointer<GameMakerList<GameMakerTextureGroupInfoResourceId>> FontIds { get; set; }

    public GameMakerPointer<GameMakerList<GameMakerTextureGroupInfoResourceId>> TilesetIds { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();

        if (context.VersionInfo.IsAtLeast(GM_2022_9)) {
            Directory = context.ReadPointerAndObject<GameMakerString>();
            Extension = context.ReadPointerAndObject<GameMakerString>();
            LoadType = (GameMakerTextureGroupInfoLoadType)context.ReadInt32();
        }

        TexturePageIds = context.ReadPointerAndObject<GameMakerList<GameMakerTextureGroupInfoResourceId>>();
        SpriteIds = context.ReadPointerAndObject<GameMakerList<GameMakerTextureGroupInfoResourceId>>();

        if (!context.VersionInfo.IsAtLeast(GM_2023_1))
            SpineSpriteIds = context.ReadPointerAndObject<GameMakerList<GameMakerTextureGroupInfoResourceId>>();

        FontIds = context.ReadPointerAndObject<GameMakerList<GameMakerTextureGroupInfoResourceId>>();
        TilesetIds = context.ReadPointerAndObject<GameMakerList<GameMakerTextureGroupInfoResourceId>>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);

        if (context.VersionInfo.IsAtLeast(GM_2022_9)) {
            context.Write(Directory);
            context.Write(Extension);
            context.Write((int)LoadType);
        }

        context.Write(TexturePageIds);
        context.Write(SpriteIds);

        if (!context.VersionInfo.IsAtLeast(GM_2023_1)) {
            if (!SpineSpriteIds.HasValue)
                throw new InvalidOperationException("SpineSpriteIds is null!");

            context.Write(SpineSpriteIds.Value);
        }

        context.Write(FontIds);
        context.Write(TilesetIds);

        context.MarkPointerAndWriteObject(TexturePageIds);
        context.MarkPointerAndWriteObject(SpriteIds);

        if (!context.VersionInfo.IsAtLeast(GM_2023_1)) {
            if (!SpineSpriteIds.HasValue)
                throw new InvalidOperationException("SpineSpriteIds is null!");

            context.MarkPointerAndWriteObject(SpineSpriteIds.Value);
        }

        context.MarkPointerAndWriteObject(FontIds);
        context.MarkPointerAndWriteObject(TilesetIds);
    }
}
