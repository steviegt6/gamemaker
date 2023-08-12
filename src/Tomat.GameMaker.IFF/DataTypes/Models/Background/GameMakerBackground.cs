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
        Transparent = context.ReadBoolean(wide: true);
        Smooth = context.ReadBoolean(wide: true);
        Preload = context.ReadBoolean(wide: true);
        Texture = context.ReadPointerAndObject<GameMakerTextureItem>();

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        TileUnknownInt32Always2 = context.ReadInt32();
        if (TileUnknownInt32Always2 != 2)
            throw new InvalidDataException($"Expected 2 for unknown int32, got {TileUnknownInt32Always2}.");

        TileWidth = context.ReadUInt32();
        TileHeight = context.ReadUInt32();
        TileOutputBorderX = context.ReadUInt32();
        TileOutputBorderY = context.ReadUInt32();
        TileColumns = context.ReadUInt32();

        var tileFrameCount = context.ReadUInt32();
        var tileCount = context.ReadUInt32();

        TileUnknownInt32Always0 = context.ReadInt32();
        if (TileUnknownInt32Always0 != 0)
            throw new InvalidDataException($"Expected 0 for unknown int32, got {TileUnknownInt32Always0}.");

        TileFrameLength = context.ReadInt64();

        Tiles = new List<List<uint>>((int)tileCount);

        for (var i = 0; i < tileCount; i++) {
            var tileFrames = new List<uint>((int)tileFrameCount);
            for (var j = 0; j < tileFrameCount; j++)
                tileFrames.Add(context.ReadUInt32());

            Tiles.Add(tileFrames);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Transparent, wide: true);
        context.Write(Smooth, wide: true);
        context.Write(Preload, wide: true);
        context.Write(Texture);

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        context.Write(TileUnknownInt32Always2);
        context.Write(TileWidth);
        context.Write(TileHeight);
        context.Write(TileOutputBorderX);
        context.Write(TileOutputBorderY);
        context.Write(TileColumns);
        context.Write((uint)Tiles![0].Count);
        context.Write((uint)Tiles!.Count);
        context.Write(TileUnknownInt32Always0);
        context.Write(TileFrameLength);

        for (var i = 0; i < Tiles.Count; i++) {
            if (i != 0 && Tiles[i].Count != Tiles[i - 1].Count)
                throw new InvalidDataException($"Tile {i} has {Tiles[i].Count} frames, but tile {i - 1} has {Tiles[i - 1].Count} frames.");

            foreach (var frame in Tiles[i])
                context.Write(frame);
        }
    }
}
