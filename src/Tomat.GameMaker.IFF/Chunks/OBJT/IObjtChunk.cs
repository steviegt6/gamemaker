using Tomat.GameMaker.IFF.DataTypes.Models.Object;

namespace Tomat.GameMaker.IFF.Chunks.OBJT;

public interface IObjtChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerObject> Objects { get; set; }
}
