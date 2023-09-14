using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.DebugCodeEntry;

// model DebugCodeEntry {
//     int numDebugInfo;
//     array {
//         int debugInfo;
//     } debugInfo;
// }

public interface IDebugCodeEntry : IGameMakerSerializable {
    List<int> DebugInfo { get; set; }
}

internal sealed class GameMakerDebugCodeEntry : IDebugCodeEntry {
    public List<int> DebugInfo { get; set; } = null!;

    public void Read(DeserializationContext context) {
        var count = context.Reader.ReadInt32();
        DebugInfo = new List<int>(count);
        for (var i = 0; i < count; i++)
            DebugInfo.Add(context.Reader.ReadInt32());
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(DebugInfo.Count);
        foreach (var info in DebugInfo)
            context.Writer.Write(info);
    }
}
