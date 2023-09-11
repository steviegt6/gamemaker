using Tomat.GameMaker.IFF.DataTypes.Models.Script;

namespace Tomat.GameMaker.IFF.Chunks.SCPT;

public interface IScptChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerScript> Scripts { get; set; }
}
