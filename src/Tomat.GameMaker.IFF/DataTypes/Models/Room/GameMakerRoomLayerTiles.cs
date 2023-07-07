using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerTiles : IGameMakerSerializable {
    public int BackgroundId { get; set; }

    public int TilesX { get; set; }

    public int TilesY { get; set; }

    public int[][]? TileData { get; set; }

    public void Read(DeserializationContext context) {
        BackgroundId = context.Reader.ReadInt32();
        TilesX = context.Reader.ReadInt32();
        TilesY = context.Reader.ReadInt32();
        TileData = new int[TilesY][];

        for (var y = 0; y < TilesY; y++) {
            TileData[y] = new int[TilesX];
            for (var x = 0; x < TilesX; x++)
                TileData[y][x] = context.Reader.ReadInt32();
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(BackgroundId);
        context.Writer.Write(TilesX);
        context.Writer.Write(TilesY);

        for (var y = 0; y < TilesY; y++)
        for (var x = 0; x < TilesX; x++)
            context.Writer.Write(TileData![y][x]);
    }
}
