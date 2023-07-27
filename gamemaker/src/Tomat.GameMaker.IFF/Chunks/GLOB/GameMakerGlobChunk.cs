using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GLOB;

internal sealed class GameMakerGlobChunk : AbstractChunk,
                                           IGlobChunk {
    public const string NAME = "GLOB";

    public List<int> GlobalCodeIds { get; set; } = null!;

    public GameMakerGlobChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        var count = context.ReadInt32();
        GlobalCodeIds = new List<int>(count);
        for (var i = 0; i < count; i++)
            GlobalCodeIds.Add(context.ReadInt32());
    }

    public override void Write(SerializationContext context) {
        context.Write(GlobalCodeIds!.Count);
        foreach (var id in GlobalCodeIds)
            context.Write(id);
    }
}
