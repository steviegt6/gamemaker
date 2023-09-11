using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes.Models.DebugCodeEntry;

namespace Tomat.GameMaker.IFF.Chunks.DBGI;

public interface IDbgiChunk : IGameMakerChunk {
    List<int> UnknownCodeEntryIndexes { get; set; }

    GameMakerPointerList<GameMakerDebugCodeEntry> CodeEntries { get; set; }
}
