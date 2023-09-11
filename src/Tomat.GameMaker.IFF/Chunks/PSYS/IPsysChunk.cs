using Tomat.GameMaker.IFF.DataTypes.Models.ParticleSystem;

namespace Tomat.GameMaker.IFF.Chunks.PSYS;

public interface IPsysChunk {
    int ChunkVersion { get; set; }

    GameMakerPointerList<GameMakerParticleSystem> ParticleSystems { get; set; }
}
