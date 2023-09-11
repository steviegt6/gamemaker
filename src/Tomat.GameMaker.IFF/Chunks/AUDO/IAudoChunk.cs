using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Audio;

namespace Tomat.GameMaker.IFF.Chunks.AUDO;

// chunk AUDO {
//     int32 numAudio;
//     array {
//         align[3];
//         Wave audio;
//     } audio;
// }

/// <summary>
///     The <c>AUDO</c> chunk, which contains audio.
/// </summary>
public interface IAudoChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of audio.
    /// </summary>
    List<GameMakerPointer<IAudio>> Audio { get; set; }
}
