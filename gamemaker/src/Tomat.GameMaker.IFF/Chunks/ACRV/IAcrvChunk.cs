using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

namespace Tomat.GameMaker.IFF.Chunks.ACRV;

public interface IAcrvChunk : IGameMakerChunk {
    int ChunkVersion { get; }

    GameMakerPointerList<GameMakerAnimationCurve> AnimationCurves { get; set; }
}
