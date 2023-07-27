using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.FilterEffect;

namespace Tomat.GameMaker.IFF.Chunks.FEDS;

public interface IFedsChunk : IGameMakerChunk {
    int ChunkVersion { get; set; }

    GameMakerPointerList<GameMakerFilterEffect> FilterEffects { get; set; }
}
