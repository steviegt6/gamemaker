using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.FilterEffect;

namespace Tomat.GameMaker.IFF.Chunks.FEDS;

/// <summary>
///     The <c>FEDS</c> chunk, which contains filter effects.
/// </summary>
public interface IFedsChunk : IGameMakerChunk {
    /// <summary>
    ///     The version of the chunk.
    /// </summary>
    /// <remarks>Expected to always be <c>1</c>.</remarks>
    int ChunkVersion { get; set; }

    /// <summary>
    ///     The list of filter effects.
    /// </summary>
    GameMakerPointerList<GameMakerFilterEffect> FilterEffects { get; set; }
}
