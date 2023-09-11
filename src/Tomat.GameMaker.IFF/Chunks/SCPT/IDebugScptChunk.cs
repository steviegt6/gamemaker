using Tomat.GameMaker.IFF.DataTypes.Models.DebugScript;

namespace Tomat.GameMaker.IFF.Chunks.SCPT;

public interface IDebugScptChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerDebugScript> Scripts { get; set; }
}
