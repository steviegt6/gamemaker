using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence;

namespace Tomat.GameMaker.IFF.Chunks.SEQN;

public sealed class GameMakerSeqnChunk : AbstractChunk {
    public const string NAME = "SEQN";

    public GameMakerPointerList<GameMakerSequence>? Sequences { get; set; }

    public GameMakerSeqnChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        // The chunk can just be empty (4-byte header and int32 length).
        if (Size == 8)
            return;

        var chunkVersion = context.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {chunkVersion}.");

        Sequences = new GameMakerPointerList<GameMakerSequence>();
        Sequences.Read(context);
    }

    public override void Write(SerializationContext context) {
        if (Sequences is null)
            return;

        context.Write(1);
        Sequences!.Write(context);
    }
}
