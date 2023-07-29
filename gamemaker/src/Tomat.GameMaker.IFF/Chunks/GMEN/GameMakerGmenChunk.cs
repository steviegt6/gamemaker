using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GMEN;

internal sealed class GameMakerGmenChunk : AbstractChunk,
                                           IGmenChunk {
    public const string NAME = "GMEN";

    public GameMakerGmenChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public List<int> EndCodeIds { get; set; } = null!;

    public override void Read(DeserializationContext context) {
        var count = context.ReadInt32();
        EndCodeIds = new List<int>(count);
        for (var i = 0; i < count; i++)
            EndCodeIds.Add(context.ReadInt32());
    }

    public override void Write(SerializationContext context) {
        context.Write(EndCodeIds.Count);
        foreach (var id in EndCodeIds)
            context.Write(id);
    }
}
