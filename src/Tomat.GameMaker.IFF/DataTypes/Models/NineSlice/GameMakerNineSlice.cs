using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.NineSlice;

public sealed class GameMakerNineSlice : IGameMakerSerializable {
    public const int TILE_MODES_LENGTH = 5;

    public int Left { get; set; }

    public int Top { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    public bool Enabled { get; set; }

    public GameMakerNineSliceTileMode[]? TileModes { get; set; }

    public void Read(DeserializationContext context) {
        Left = context.Reader.ReadInt32();
        Top = context.Reader.ReadInt32();
        Right = context.Reader.ReadInt32();
        Bottom = context.Reader.ReadInt32();
        Enabled = context.Reader.ReadBoolean(wide: true);

        TileModes = new GameMakerNineSliceTileMode[TILE_MODES_LENGTH];
        for (var i = 0; i < TILE_MODES_LENGTH; i++)
            TileModes[i] = (GameMakerNineSliceTileMode)context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Left);
        context.Writer.Write(Top);
        context.Writer.Write(Right);
        context.Writer.Write(Bottom);
        context.Writer.Write(Enabled, wide: true);
        for (var i = 0; i < TILE_MODES_LENGTH; i++)
            context.Writer.Write((int)TileModes![i]);
    }
}
