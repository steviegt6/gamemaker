using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.DebugCodeEntry;

namespace Tomat.GameMaker.IFF.Chunks.DBGI;

// chunk DBGI {
//    int32 numCodeEntries;
//    array {
//        int32 vmIndex;
//    } vmIndices;
//    int32 numCode;
//    array {
//        DebugCodeEntry* code;
//    } codeEntries;
// }

public interface IDbgiChunk : IGameMakerChunk {
    List<int> VmIndices { get; set; }

    List<GameMakerPointer<IDebugCodeEntry>> CodeEntries { get; set; }
}

internal sealed class GameMakerDbgiChunk : AbstractChunk,
                                           IDbgiChunk {
    public const string NAME = "DBGI";

    public List<int> VmIndices { get; set; } = null!;

    public List<GameMakerPointer<IDebugCodeEntry>> CodeEntries { get; set; } = null!;

    public GameMakerDbgiChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        var count = context.Reader.ReadInt32();
        VmIndices = new List<int>(count);
        for (var i = 0; i < count; i++)
            VmIndices.Add(context.Reader.ReadInt32());
        CodeEntries = context.ReadPointerList<IDebugCodeEntry, GameMakerDebugCodeEntry>();
    }

    public override void Write(SerializationContext context) {
        context.Writer.Write(VmIndices.Count);
        foreach (var entry in VmIndices)
            context.Writer.Write(entry);
        context.WritePointerList(CodeEntries);
    }
}
