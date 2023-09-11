using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

namespace Tomat.GameMaker.IFF.Chunks.ACRV;

// chunk ACRV {
//     align[4];
//     int32 version;
//     list<model {
//         GMAnimationCurve curve;
//         align[4];
//     }*> curves;
// }

/// <summary>
///     The <c>ACRV</c> chunk, which contains animation curves.
/// </summary>
public interface IAcrvChunk : IGameMakerChunk {
    /// <summary>
    ///     The version of the chunk.
    /// </summary>
    /// <remarks>Expected to always be <c>1</c>.</remarks>
    int ChunkVersion { get; }

    /// <summary>
    ///     The list of animation curves.
    /// </summary>
    List<GameMakerPointer<IAnimationCurve>> AnimationCurves { get; set; }
}
