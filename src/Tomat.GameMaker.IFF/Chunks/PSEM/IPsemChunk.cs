using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.ParticleSystemEmitter;

namespace Tomat.GameMaker.IFF.Chunks.PSEM;

public interface IPsemChunk : IGameMakerChunk {
    int ChunkVersion { get; set; }

    GameMakerPointerList<GameMakerParticleSystemEmitter> ParticleSystemEmitters { get; set; }
}
