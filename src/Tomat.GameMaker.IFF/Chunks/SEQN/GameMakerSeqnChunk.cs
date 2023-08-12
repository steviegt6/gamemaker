using System.IO;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence;

namespace Tomat.GameMaker.IFF.Chunks.SEQN;

internal sealed class GameMakerSeqnChunk : AbstractChunk,
                                           ISeqnChunk {
    public const string NAME = "SEQN";

    public GameMakerSeqnChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        // The chunk can just be empty (4-byte header and int32 length).
        if (Size == 0)
            return;

        var chunkVersion = context.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {chunkVersion}.");

        var sequences = context.ReadPointerList<GameMakerSequence>();
        AddComponent<ISeqnChunkSequenceComponent>(new SeqnChunkSequenceComponent {
            ChunkVersion = chunkVersion,
            Sequences = sequences,
        });
    }

    public override void Write(SerializationContext context) {
        if (!TryGetComponent<ISeqnChunkSequenceComponent>(out var sequenceComponent))
            return;

        if (sequenceComponent.ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {sequenceComponent.ChunkVersion}.");

        context.Write(sequenceComponent.ChunkVersion);
        context.Write(sequenceComponent.Sequences);
    }
}
