using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Sound;

namespace Tomat.GameMaker.IFF.Chunks.SOND;

public interface ISondChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerSound> Sounds { get; set; }
}
