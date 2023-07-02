using System;
using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

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
        Width = context.Reader.ReadInt32();
        Height = context.Reader.ReadInt32();
        MarginLeft = context.Reader.ReadInt32();
        MarginRight = context.Reader.ReadInt32();
        MarginBottom = context.Reader.ReadInt32();
        MarginTop = context.Reader.ReadInt32();
        Transparent = context.Reader.ReadBoolean(wide: true);
        Smooth = context.Reader.ReadBoolean(wide: true);
        Preload = context.Reader.ReadBoolean(wide: true);
        BBoxMode = context.Reader.ReadUInt32();
        SeparationMaskTypes = (GameMakerSpriteSeparationMaskType)context.Reader.ReadUInt32();
        OriginX = context.Reader.ReadInt32();
        OriginY = context.Reader.ReadInt32();

        TextureItems = new GameMakerRemotePointerList<GameMakerTextureItem>();

        if (context.Reader.ReadInt32() == -1) {
            SpecialOrGm2 = true;

            SpriteVersion = context.Reader.ReadInt32();
            SpriteType = (GameMakerSpriteType)context.Reader.ReadInt32();

            if (context.VersionInfo.IsAtLeast(GM_2)) {
                PlaybackSpeed = context.Reader.ReadSingle();
                PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.Reader.ReadInt32();

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
                    TextureItems.Read(context);
                    ParseMaskData(context);
                    break;

                case GameMakerSpriteType.Swf:
                    if (context.Reader.ReadInt32() != 8)
                        throw new InvalidDataException("Expected 8, SWF format incorrect.");

                    TextureItems.Read(context);
                    context.Reader.Pad(4);
                    begin = context.Reader.Position;
                    var jpegTablesLength = context.Reader.ReadInt32();
                    if (context.Reader.ReadInt32() != 8)
                        throw new InvalidDataException("Expected 8, SWF format incorrect.");

                    context.Reader.Position += jpegTablesLength;
                    context.Reader.Pad(4);
                    context.Reader.Position += (context.Reader.ReadInt32() * 8) + 4;
                    var frameCount = context.Reader.ReadInt32();
                    context.Reader.Position += 16;
                    var maskCount = context.Reader.ReadInt32();
                    context.Reader.Position += 8;
                    for (var i = 0; i < frameCount; i++)
                        context.Reader.Position += (context.Reader.ReadInt32() * 100) + 16;

                    for (var i = 0; i < maskCount; i++) {
                        context.Reader.Position += context.Reader.ReadInt32();
                        context.Reader.Pad(4);
                    }

                    var swfDataLength = context.Reader.Position - begin;
                    context.Reader.Position = begin;
                    SpriteBuffer = context.Reader.ReadBytes(swfDataLength);
                    break;

                case GameMakerSpriteType.Spine:
                    context.Reader.Pad(4);

                    begin = context.Reader.Position;
                    _ = context.Reader.ReadUInt32(); // Version number.
                    var jsonLength = context.Reader.ReadInt32();
                    var atlasLength = context.Reader.ReadInt32();
                    var textureLength = context.Reader.ReadInt32();
                    _ = context.Reader.ReadUInt32(); // Atlas texture width.
                    _ = context.Reader.ReadUInt32(); // Atlas texture height.
                    context.Reader.Position = begin;

                    SpriteBuffer = context.Reader.ReadBytes((sizeof(uint) * 3) + (sizeof(int) * 3) + jsonLength + atlasLength + textureLength);
                    break;

                default:
                    throw new InvalidDataException($"Unknown sprite type {SpriteType.ToString()}.");
            }
        }
        else {
            // Normal, GameMaker 1.4 sprite.
            context.Reader.Position -= 4;
            TextureItems.Read(context);
            ParseMaskData(context);
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Width);
        context.Writer.Write(Height);
        context.Writer.Write(MarginLeft);
        context.Writer.Write(MarginRight);
        context.Writer.Write(MarginBottom);
        context.Writer.Write(MarginTop);
        context.Writer.Write(Transparent, wide: true);
        context.Writer.Write(Smooth, wide: true);
        context.Writer.Write(Preload, wide: true);
        context.Writer.Write(BBoxMode);
        context.Writer.Write((uint)SeparationMaskTypes);
        context.Writer.Write(OriginX);
        context.Writer.Write(OriginY);

        if (SpecialOrGm2) {
            context.Writer.Write(-1);
            if (context.VersionInfo.IsAtLeast(GM_2_3_2))
                context.Writer.Write(3);
            else if (context.VersionInfo.IsAtLeast(GM_2_3))
                context.Writer.Write(2);
            else
                context.Writer.Write(1);
            context.Writer.Write((int)SpriteType);

            if (context.VersionInfo.IsAtLeast(GM_2)) {
                context.Writer.Write(PlaybackSpeed);
                context.Writer.Write((int)PlaybackSpeedType);

                if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                    context.Writer.Write(Sequence);

                    if (context.VersionInfo.IsAtLeast(GM_2_3_2))
                        context.Writer.Write(NineSlice);
                }
            }

            switch (SpriteType) {
                case GameMakerSpriteType.Normal:
                    TextureItems!.Write(context);
                    WriteMaskData(context);
                    break;

                case GameMakerSpriteType.Swf:
                    context.Writer.Write(8);
                    TextureItems!.Write(context);
                    context.Writer.Pad(4);
                    context.Writer.Write(SpriteBuffer);
                    break;

                case GameMakerSpriteType.Spine:
                    context.Writer.Pad(4);
                    context.Writer.Write(SpriteBuffer);
                    break;

                default:
                    throw new InvalidDataException($"Unknown sprite type {SpriteType.ToString()}.");
            }

            if (Sequence is { IsNull: false, Object: { } }) {
                context.Writer.Pad(4);
                Sequence.WriteObject(context);
                Sequence.Object.Write(context);
            }

            if (NineSlice is { IsNull: false, Object: { } }) {
                context.Writer.Pad(4);
                NineSlice.WriteObject(context);
                NineSlice.Object.Write(context);
            }
        }
        else {
            TextureItems!.Write(context);
            WriteMaskData(context);
        }
    }

    private void ParseMaskData(DeserializationContext context) {
        var maskCount = context.Reader.ReadInt32();
        var length = ((Width + 7) / 8) * Height;

        CollisionMasks = new List<Memory<byte>>();
        var total = 0;

        for (var i = 0; i < maskCount; i++) {
            CollisionMasks.Add(context.Reader.ReadBytes(length));
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
        context.Writer.Write(CollisionMasks!.Count);
        var total = 0;

        foreach (var mask in CollisionMasks) {
            context.Writer.Write(mask);
            total += mask.Length;
        }

        // Pad to 4 bytes.
        if (total % 4 != 0)
            context.Writer.Pad(4 - (total % 4));
        context.Writer.Pad(4);

        var totalBits = (Width + 7) / 8 * 8 * Height * CollisionMasks.Count;
        var totalBytes = ((totalBits + 31) / 32 * 32) / 8;
        if (total != totalBytes)
            throw new InvalidDataException("Mask data length mismatch.");
    }
}
