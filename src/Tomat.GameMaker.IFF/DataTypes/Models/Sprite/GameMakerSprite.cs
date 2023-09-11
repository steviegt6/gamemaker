using System;
using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.NineSlice;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sprite;

public sealed class GameMakerSprite : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int MarginLeft { get; set; }

    public int MarginRight { get; set; }

    public int MarginBottom { get; set; }

    public int MarginTop { get; set; }

    public bool Transparent { get; set; }

    public bool Smooth { get; set; }

    public bool Preload { get; set; }

    public uint BBoxMode { get; set; }

    public GameMakerSpriteSeparationMaskType SeparationMaskTypes { get; set; }

    public int OriginX { get; set; }

    public int OriginY { get; set; }

    /// <summary>
    ///     Indicates that this sprite is special/a GameMaker 2 sprite, just
    ///     meaning it has some extra data (<see cref="SpriteType"/>,
    ///     <see cref="PlaybackSpeed"/>, <see cref="PlaybackSpeedType"/>,
    ///     <see cref="Sequence"/>, and <see cref="NineSlice"/>).
    /// </summary>
    public bool SpecialOrGm2 { get; set; }

    /// <summary>
    ///     Related to <see cref="SpecialOrGm2"/>; indicates
    /// </summary>
    public int SpriteVersion { get; set; }

    public GameMakerSpriteType SpriteType { get; set; }

    public Memory<byte> SpriteBuffer { get; set; }

    public float PlaybackSpeed { get; set; }

    public GameMakerSpritePlaybackSpeedType PlaybackSpeedType { get; set; }

    public GameMakerPointer<GameMakerSpriteSequenceReference> Sequence { get; set; }

    public GameMakerPointer<GameMakerNineSlice> NineSlice { get; set; }

    public GameMakerRemotePointerList<GameMakerTextureItem>? TextureItems { get; set; }

    public List<Memory<byte>>? CollisionMasks { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Width = context.ReadInt32();
        Height = context.ReadInt32();
        MarginLeft = context.ReadInt32();
        MarginRight = context.ReadInt32();
        MarginBottom = context.ReadInt32();
        MarginTop = context.ReadInt32();
        Transparent = context.ReadBoolean(wide: true);
        Smooth = context.ReadBoolean(wide: true);
        Preload = context.ReadBoolean(wide: true);
        BBoxMode = context.ReadUInt32();
        SeparationMaskTypes = (GameMakerSpriteSeparationMaskType)context.ReadUInt32();
        OriginX = context.ReadInt32();
        OriginY = context.ReadInt32();

        if (context.ReadInt32() == -1) {
            SpecialOrGm2 = true;

            SpriteVersion = context.ReadInt32();
            SpriteType = (GameMakerSpriteType)context.ReadInt32();

            if (context.VersionInfo.IsAtLeast(GM_2)) {
                PlaybackSpeed = context.ReadSingle();
                PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.ReadInt32();

                if (SpriteVersion >= 2) {
                    Sequence = context.ReadPointerAndObject<GameMakerSpriteSequenceReference>();

                    if (SpriteVersion >= 3) {
                        context.VersionInfo.UpdateTo(GM_2_3_2);
                        NineSlice = context.ReadPointerAndObject<GameMakerNineSlice>();
                    }
                }
            }

            int begin;

            switch (SpriteType) {
                case GameMakerSpriteType.Normal:
                    TextureItems = new GameMakerRemotePointerList<GameMakerTextureItem>();
                    TextureItems.Read(context);
                    ParseMaskData(context);
                    break;

                case GameMakerSpriteType.Swf:
                    if (context.ReadInt32() != 8)
                        throw new InvalidDataException("Expected 8, SWF format incorrect.");

                    TextureItems = new GameMakerRemotePointerList<GameMakerTextureItem>();
                    TextureItems.Read(context);
                    context.Align(4);
                    begin = context.Position;
                    var jpegTablesLength = context.ReadInt32();
                    if (context.ReadInt32() != 8)
                        throw new InvalidDataException("Expected 8, SWF format incorrect.");

                    context.Position += jpegTablesLength;
                    context.Align(4);
                    context.Position += (context.ReadInt32() * 8) + 4;
                    var frameCount = context.ReadInt32();
                    context.Position += 16;
                    var maskCount = context.ReadInt32();
                    context.Position += 8;
                    for (var i = 0; i < frameCount; i++)
                        context.Position += (context.ReadInt32() * 100) + 16;

                    for (var i = 0; i < maskCount; i++) {
                        context.Position += context.ReadInt32();
                        context.Align(4);
                    }

                    var swfDataLength = context.Position - begin;
                    context.Position = begin;
                    SpriteBuffer = context.ReadBytes(swfDataLength);
                    break;

                case GameMakerSpriteType.Spine:
                    context.Align(4);

                    begin = context.Position;
                    _ = context.ReadUInt32(); // Version number.
                    var jsonLength = context.ReadInt32();
                    var atlasLength = context.ReadInt32();
                    var textureLength = context.ReadInt32();
                    _ = context.ReadUInt32(); // Atlas texture width.
                    _ = context.ReadUInt32(); // Atlas texture height.
                    context.Position = begin;

                    SpriteBuffer = context.ReadBytes((sizeof(uint) * 3) + (sizeof(int) * 3) + jsonLength + atlasLength + textureLength);
                    break;

                default:
                    throw new InvalidDataException($"Unknown sprite type {SpriteType.ToString()}.");
            }
        }
        else {
            // Normal, GameMaker 1.4 sprite.
            context.Position -= 4;
            TextureItems = new GameMakerRemotePointerList<GameMakerTextureItem>();
            TextureItems.Read(context);
            ParseMaskData(context);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Width);
        context.Write(Height);
        context.Write(MarginLeft);
        context.Write(MarginRight);
        context.Write(MarginBottom);
        context.Write(MarginTop);
        context.Write(Transparent, wide: true);
        context.Write(Smooth, wide: true);
        context.Write(Preload, wide: true);
        context.Write(BBoxMode);
        context.Write((uint)SeparationMaskTypes);
        context.Write(OriginX);
        context.Write(OriginY);

        if (SpecialOrGm2) {
            context.Write(-1);
            if (context.VersionInfo.IsAtLeast(GM_2_3_2))
                context.Write(3);
            else if (context.VersionInfo.IsAtLeast(GM_2_3))
                context.Write(2);
            else
                context.Write(1);
            context.Write((int)SpriteType);

            if (context.VersionInfo.IsAtLeast(GM_2)) {
                context.Write(PlaybackSpeed);
                context.Write((int)PlaybackSpeedType);

                if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                    context.Write(Sequence);

                    if (context.VersionInfo.IsAtLeast(GM_2_3_2))
                        context.Write(NineSlice);
                }
            }

            switch (SpriteType) {
                case GameMakerSpriteType.Normal:
                    if (TextureItems is null)
                        throw new InvalidOperationException("TextureItems is null.");

                    context.Write(TextureItems);
                    WriteMaskData(context);
                    break;

                case GameMakerSpriteType.Swf:
                    context.Write(8);
                    if (TextureItems is null)
                        throw new InvalidOperationException("TextureItems is null.");

                    context.Write(TextureItems);
                    context.Align(4);
                    context.Write(SpriteBuffer);
                    break;

                case GameMakerSpriteType.Spine:
                    context.Align(4);
                    context.Write(SpriteBuffer);
                    break;

                default:
                    throw new InvalidDataException($"Unknown sprite type {SpriteType.ToString()}.");
            }

            if (!Sequence.IsNull) {
                context.Align(4);
                context.MarkPointerAndWriteObject(Sequence);
            }

            if (!NineSlice.IsNull) {
                context.Align(4);
                context.MarkPointerAndWriteObject(NineSlice);
            }
        }
        else {
            if (TextureItems is null)
                throw new InvalidOperationException("TextureItems is null.");

            context.Write(TextureItems);
            WriteMaskData(context);
        }
    }

    private void ParseMaskData(DeserializationContext context) {
        var maskCount = context.ReadInt32();
        var length = ((Width + 7) / 8) * Height;

        CollisionMasks = new List<Memory<byte>>();
        var total = 0;

        for (var i = 0; i < maskCount; i++) {
            CollisionMasks.Add(context.ReadBytes(length));
            total += length;
        }

        // Pad to 4 bytes
        if (total % 4 != 0)
            total += 4 - (total % 4);

        var totalBits = ((Width + 7) / 8 * 8) * Height * maskCount;
        var totalBytes = ((totalBits + 31) / 32 * 32) / 8;
        if (total != totalBytes)
            throw new InvalidDataException("Mask data length mismatch.");
    }

    private void WriteMaskData(SerializationContext context) {
        context.Write(CollisionMasks!.Count);
        var total = 0;

        foreach (var mask in CollisionMasks) {
            context.Write(mask);
            total += mask.Length;
        }

        // Pad to 4 bytes.
        if (total % 4 != 0)
            total += 4 - (total % 4);
        context.Align(4);

        var totalBits = (Width + 7) / 8 * 8 * Height * CollisionMasks.Count;
        var totalBytes = ((totalBits + 31) / 32 * 32) / 8;
        if (total != totalBytes)
            throw new InvalidDataException("Mask data length mismatch.");
    }
}
