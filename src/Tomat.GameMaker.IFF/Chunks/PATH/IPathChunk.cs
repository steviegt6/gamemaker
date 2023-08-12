using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Path;

namespace Tomat.GameMaker.IFF.Chunks.PATH;

public interface IPathChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerPath> Paths { get; set; }
}
