using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.ParticleSystem;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.PSYS;

internal sealed class GameMakerPsysChunk : AbstractChunk,
                                           IPsysChunk {
    public const string NAME = "PSYS";

    public int ChunkVersion { get; set; }

    public GameMakerPointerList<GameMakerParticleSystem> ParticleSystems { get; set; } = null!;

    public GameMakerPsysChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.Pad(4);
        ChunkVersion = context.ReadInt32();
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        ParticleSystems = context.ReadPointerList<GameMakerParticleSystem>();
    }

    public override void Write(SerializationContext context) {
        context.Pad(4);

        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        context.Write(ChunkVersion);
        context.Write(ParticleSystems);
    }
}
