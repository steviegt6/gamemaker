using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GLOB;

// chunk GLOB {
//     int32 numGlobalCode;
//     array {
//         int32 globalCode;
//     } globalCode;
// }

public interface IGlobChunk : IGameMakerChunk {
    List<int> GlobalCodeIds { get; }
}
