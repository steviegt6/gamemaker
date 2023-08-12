using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.DebugCodeEntry;

public sealed class GameMakerDebugCodeEntry : IGameMakerSerializable {
    public List<int> UnknownDebugInfo { get; set; } = null!;

    public void Read(DeserializationContext context) {
        var count = context.Reader.ReadInt32();
        UnknownDebugInfo = new List<int>(count);
        for (var i = 0; i < count; i++)
            UnknownDebugInfo.Add(context.Reader.ReadInt32());
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(UnknownDebugInfo.Count);
        foreach (var info in UnknownDebugInfo)
            context.Writer.Write(info);
    }
}
