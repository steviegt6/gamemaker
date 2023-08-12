using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Room;

namespace Tomat.GameMaker.IFF.Chunks.ROOM;

public interface IRoomChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerRoom> Rooms { get; set; }
}
