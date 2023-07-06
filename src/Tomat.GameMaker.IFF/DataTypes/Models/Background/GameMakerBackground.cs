using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Background;

public sealed class GameMakerBackground : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public bool Transparent { get; set; }

    public bool Smooth { get; set; }

    public bool Preload { get; set; }

    public GameMakerPointer<GameMakerTextureItem> Texture { get; set; }

    public int TileUnknownInt32Always2 { get; set; }

    public uint TileWidth { get; set; }

    public uint TileHeight { get; set; }

    public uint TileOutputBorderX { get; set; }

    public uint TileOutputBorderY { get; set; }

    public uint TileColumns { get; set; }

    public int TileUnknownInt32Always0 { get; set; }

    public long TileFrameLength { get; set; }

    public List<List<uint>>? Tiles { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Transparent = context.Reader.ReadBoolean(wide: true);
        Smooth = context.Reader.ReadBoolean(wide: true);
        Preload = context.Reader.ReadBoolean(wide: true);
        Texture = context.ReadPointerAndObject<GameMakerTextureItem>();

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        TileUnknownInt32Always2 = context.Reader.ReadInt32();
        if (TileUnknownInt32Always2 != 2)
            throw new InvalidDataException($"Expected 2 for unknown int32, got {TileUnknownInt32Always2}.");

        TileWidth = context.Reader.ReadUInt32();
        TileHeight = context.Reader.ReadUInt32();
        TileOutputBorderX = context.Reader.ReadUInt32();
        TileOutputBorderY = context.Reader.ReadUInt32();
        TileColumns = context.Reader.ReadUInt32();

        var tileFrameCount = context.Reader.ReadUInt32();
        var tileCount = context.Reader.ReadUInt32();

        TileUnknownInt32Always0 = context.Reader.ReadInt32();
        if (TileUnknownInt32Always0 != 0)
            throw new InvalidDataException($"Expected 0 for unknown int32, got {TileUnknownInt32Always0}.");

        TileFrameLength = context.Reader.ReadInt64();

        Tiles = new List<List<uint>>((int)tileCount);

        for (var i = 0; i < tileCount; i++) {
            var tileFrames = new List<uint>((int)tileFrameCount);
            for (var j = 0; j < tileFrameCount; j++)
                tileFrames.Add(context.Reader.ReadUInt32());

            Tiles.Add(tileFrames);
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Transparent, wide: true);
        context.Writer.Write(Smooth, wide: true);
        context.Writer.Write(Preload, wide: true);
        context.Writer.Write(Texture);

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        context.Writer.Write(TileUnknownInt32Always2);
        context.Writer.Write(TileWidth);
        context.Writer.Write(TileHeight);
        context.Writer.Write(TileOutputBorderX);
        context.Writer.Write(TileOutputBorderY);
        context.Writer.Write(TileColumns);
        context.Writer.Write((uint)Tiles![0].Count);
        context.Writer.Write((uint)Tiles!.Count);
        context.Writer.Write(TileUnknownInt32Always0);
        context.Writer.Write(TileFrameLength);

        for (var i = 0; i < Tiles.Count; i++) {
            if (i != 0 && Tiles[i].Count != Tiles[i - 1].Count)
                throw new InvalidDataException($"Tile {i} has {Tiles[i].Count} frames, but tile {i - 1} has {Tiles[i - 1].Count} frames.");

            foreach (var frame in Tiles[i])
                context.Writer.Write(frame);
        }
    }
}
