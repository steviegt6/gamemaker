using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GLOB;

public sealed class GameMakerGlobChunk : AbstractChunk {
    public const string NAME = "GLOB";

    public List<int>? GlobalCodeIds { get; set; }

    public GameMakerGlobChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        var count = context.Reader.ReadInt32();
        GlobalCodeIds = new List<int>(count);
        for (var i = 0; i < count; i++)
            GlobalCodeIds.Add(context.Reader.ReadInt32());
    }

    public override void Write(SerializationContext context) {
        context.Writer.Write(GlobalCodeIds!.Count);
        foreach (var id in GlobalCodeIds)
            context.Writer.Write(id);
    }
}
