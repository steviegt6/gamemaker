using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence;

namespace Tomat.GameMaker.IFF.Chunks.SEQN;

public interface ISeqnChunkSequenceComponent {
    int ChunkVersion { get; set; }

    GameMakerPointerList<GameMakerSequence> Sequences { get; set; }
}

internal sealed class SeqnChunkSequenceComponent : ISeqnChunkSequenceComponent {
    public required int ChunkVersion { get; set; }

    public required GameMakerPointerList<GameMakerSequence>? Sequences { get; set; }
}
