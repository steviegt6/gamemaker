using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GMEN;

// chunk GMEN {
//     int32 numEndCode;
//     array {
//         int32 endCode;
//     } endCode;
// }

/// <summary>
///     The <c>GMEN</c> chunk, which contains the game's end code entries.
/// </summary>
public interface IGmenChunk : IGameMakerChunk {
    List<int> EndCodeIds { get; set; }
}
