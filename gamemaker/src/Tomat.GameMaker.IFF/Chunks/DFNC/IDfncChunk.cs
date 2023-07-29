using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.DebugFunctionInfo;

namespace Tomat.GameMaker.IFF.Chunks.DFNC;

public interface IDfncChunk {
    int ChunkVersion { get; set; }

    GameMakerPointerList<GameMakerDebugFunctionInfo> Functions { get; set; }
}
