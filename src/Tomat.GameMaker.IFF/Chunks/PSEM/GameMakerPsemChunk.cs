using System.IO;
using Tomat.GameMaker.IFF.DataTypes.Models.ParticleSystemEmitter;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.PSEM;

internal sealed class GameMakerPsemChunk : AbstractChunk,
                                           IPsemChunk {
    public const string NAME = "PSEM";

    public int ChunkVersion { get; set; }

    public GameMakerPointerList<GameMakerParticleSystemEmitter> ParticleSystemEmitters { get; set; } = null!;

    public GameMakerPsemChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.Align(4);

        ChunkVersion = context.ReadInt32();
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        ParticleSystemEmitters = context.ReadPointerList<GameMakerParticleSystemEmitter>();
    }

    public override void Write(SerializationContext context) {
        context.Align(4);

        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        context.Write(ChunkVersion);
        context.Write(ParticleSystemEmitters);
    }
}
