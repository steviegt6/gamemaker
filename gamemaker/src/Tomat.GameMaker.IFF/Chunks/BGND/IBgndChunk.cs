using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Background;

namespace Tomat.GameMaker.IFF.Chunks.BGND;

public interface IBgndChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerBackground> Backgrounds { get; set; }
}
