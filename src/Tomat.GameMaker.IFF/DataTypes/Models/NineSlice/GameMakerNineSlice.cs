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
        Left = context.ReadInt32();
        Top = context.ReadInt32();
        Right = context.ReadInt32();
        Bottom = context.ReadInt32();
        Enabled = context.ReadBoolean(wide: true);

        TileModes = new GameMakerNineSliceTileMode[TILE_MODES_LENGTH];
        for (var i = 0; i < TILE_MODES_LENGTH; i++)
            TileModes[i] = (GameMakerNineSliceTileMode)context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(Left);
        context.Write(Top);
        context.Write(Right);
        context.Write(Bottom);
        context.Write(Enabled, wide: true);
        for (var i = 0; i < TILE_MODES_LENGTH; i++)
            context.Write((int)TileModes![i]);
    }
}
