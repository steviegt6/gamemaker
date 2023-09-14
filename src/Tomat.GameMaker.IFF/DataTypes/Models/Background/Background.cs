using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Background;

// model GMBackground {
//     string* name;
//     bool transparent;
//     bool smooth;
//     bool preload;
//     TexturePageEntry* texturePageEntry;
// #if VERSION >= 2.0.0
//     int32 const2; // always 2
//     int tileWidth;
//     int tileHeight;
//     int tileHSep;
//     int tileVSep;
//     int columns;
//     int frames;
//     int tileCount;
//     int32 const0; // always 0
//     long frameLength; // numFrameData
//     array {
//         uint
//     } frameData;
// #endif
// }

public interface IBackground : IGameMakerSerializable {
    GameMakerPointer<IString> Name { get; set; }

    bool Transparent { get; set; }

    bool Smooth { get; set; }

    bool Preload { get; set; }

    // Can be null.
    GameMakerPointer<ITexturePageEntry> TexturePageEntry { get; set; }
}

public interface IBackgroundGm2Component {
    int Const2 { get; }

    int TileWidth { get; set; }

    int TileHeight { get; set; }

    int TileHSep { get; set; }

    int TileVSep { get; set; }

    int Columns { get; set; }

    int Frames { get; set; }

    int TileCount { get; }

    int Const0 { get; }

    long FrameLength { get; set; }

    List<List<uint>> FrameData { get; set; }
}

internal sealed class GameMakerBackground : AbstractSerializableWithComponents,
                                            IBackground {
    internal sealed class GameMakerBackgroundGm2Component : IBackgroundGm2Component {
        public required int Const2 { get; init; }

        public required int TileWidth { get; set; }

        public required int TileHeight { get; set; }

        public required int TileHSep { get; set; }

        public required int TileVSep { get; set; }

        public required int Columns { get; set; }

        public required int Frames { get; set; }

        public int TileCount => FrameData.Count;

        public required int Const0 { get; init; }

        public required long FrameLength { get; set; }

        public required List<List<uint>> FrameData { get; set; }
    }

    public const int CONST_2 = 2;
    public const int CONST_0 = 0;

    public GameMakerPointer<IString> Name { get; set; }

    public bool Transparent { get; set; }

    public bool Smooth { get; set; }

    public bool Preload { get; set; }

    public GameMakerPointer<ITexturePageEntry> TexturePageEntry { get; set; }

    public override void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<IString, GameMakerString>();
        Transparent = context.ReadBoolean(wide: true);
        Smooth = context.ReadBoolean(wide: true);
        Preload = context.ReadBoolean(wide: true);
        TexturePageEntry = context.ReadPointerAndObject<ITexturePageEntry, GameMakerTexturePageEntry>();

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        var const2 = context.ReadInt32().Expect(CONST_2, x => new InvalidDataException($"Expected {CONST_2}, got {x}."));
        var tileWidth = context.ReadInt32();
        var tileHeight = context.ReadInt32();
        var tileHSep = context.ReadInt32();
        var tileVSep = context.ReadInt32();
        var columns = context.ReadInt32();
        var frames = context.ReadInt32();
        var tileCount = context.ReadInt32();
        var const0 = context.ReadInt32().Expect(CONST_0, x => new InvalidDataException($"Expected {CONST_0}, got {x}."));
        var frameLength = context.ReadInt64();
        var frameData = new List<List<uint>>(tileCount);

        for (var i = 0; i < tileCount; i++) {
            var tileFrames = new List<uint>(frames);
            for (var j = 0; j < frames; j++)
                tileFrames.Add(context.ReadUInt32());

            frameData.Add(tileFrames);
        }

        AddComponent<IBackgroundGm2Component>(
            new GameMakerBackgroundGm2Component {
                Const2 = const2,
                TileWidth = tileWidth,
                TileHeight = tileHeight,
                TileHSep = tileHSep,
                TileVSep = tileVSep,
                Columns = columns,
                Frames = frames,
                Const0 = const0,
                FrameLength = frameLength,
                FrameData = frameData,
            }
        );
    }

    public override void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Transparent, wide: true);
        context.Write(Smooth, wide: true);
        context.Write(Preload, wide: true);
        context.Write(TexturePageEntry);

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        if (!TryGetComponent<IBackgroundGm2Component>(out var gm2Component))
            throw new InvalidDataException("GM2 component not found.");

        context.Write(gm2Component.Const2.Expect(CONST_2, new InvalidDataException($"Expected {CONST_2}, got {gm2Component.Const2}.")));
        context.Write(gm2Component.TileWidth);
        context.Write(gm2Component.TileHeight);
        context.Write(gm2Component.TileHSep);
        context.Write(gm2Component.TileVSep);
        context.Write(gm2Component.Columns);
        context.Write(gm2Component.Frames);
        context.Write(gm2Component.TileCount);
        context.Write(gm2Component.Const0.Expect(CONST_0, new InvalidDataException($"Expected {CONST_0}, got {gm2Component.Const0}.")));
        context.Write(gm2Component.FrameLength);

        var frameData = gm2Component.FrameData;

        for (var i = 0; i < frameData.Count; i++) {
            if (i != 0 && frameData[i].Count != frameData[i - 1].Count)
                throw new InvalidDataException($"Tile {i} has {frameData[i].Count} frames, but tile {i - 1} has {frameData[i - 1].Count} frames.");

            foreach (var frame in frameData[i])
                context.Write(frame);
        }
    }
}
