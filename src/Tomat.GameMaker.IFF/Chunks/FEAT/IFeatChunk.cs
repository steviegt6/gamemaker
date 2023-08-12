using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.FEAT;

/// <summary>
///     The <c>FEAT</c> chunk, which contains feature flags.
/// </summary>
public interface IFeatChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of feature flags.
    /// </summary>
    List<GameMakerPointer<GameMakerString>> FeatureFlags { get; set; }
}
