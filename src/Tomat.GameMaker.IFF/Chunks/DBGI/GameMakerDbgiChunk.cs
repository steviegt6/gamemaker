using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes.Models.DebugCodeEntry;

namespace Tomat.GameMaker.IFF.Chunks.DBGI;

internal sealed class GameMakerDbgiChunk : AbstractChunk,
                                           IDbgiChunk {
    public const string NAME = "DBGI";

    public List<int> UnknownCodeEntryIndexes { get; set; } = null!;

    public GameMakerPointerList<GameMakerDebugCodeEntry> CodeEntries { get; set; } = null!;

    public GameMakerDbgiChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        var count = context.Reader.ReadInt32();
        UnknownCodeEntryIndexes = new List<int>(count);
        for (var i = 0; i < count; i++)
            UnknownCodeEntryIndexes.Add(context.Reader.ReadInt32());
        CodeEntries = context.ReadPointerList<GameMakerDebugCodeEntry>();
    }

    public override void Write(SerializationContext context) {
        context.Writer.Write(UnknownCodeEntryIndexes.Count);
        foreach (var index in UnknownCodeEntryIndexes)
            context.Writer.Write(index);
        context.Write(CodeEntries);
    }
}
