using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.AGRP;

// chunk AGRP {
//     int numAudioGroups;
//    array {
//         string* audioGroup;
//    } audioGroups;
// }

/// <summary>
///     The <c>AGRP</c> chunk, which contains audio groups.
/// </summary>
public interface IAgrpChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of audio groups.
    /// </summary>
    List<GameMakerPointer<IString>> AudioGroups { get; set; }
}
