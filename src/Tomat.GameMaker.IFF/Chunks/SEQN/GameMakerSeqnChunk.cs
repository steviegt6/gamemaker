using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence;

namespace Tomat.GameMaker.IFF.Chunks.SEQN;

public sealed class GameMakerSeqnChunk : AbstractChunk {
    public const string NAME = "SEQN";

    // This list can be uniquely null even if the chunk was read.
    public GameMakerPointerList<GameMakerSequence>? Sequences { get; set; }

    public GameMakerSeqnChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        // The chunk can just be empty (4-byte header and int32 length).
        if (Size == 0)
            return;

        var chunkVersion = context.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {chunkVersion}.");

        Sequences = context.ReadPointerList<GameMakerSequence>();
    }

    public override void Write(SerializationContext context) {
        if (Sequences is null)
            return;

        context.Write(1);
        context.Write(Sequences);
    }
}
