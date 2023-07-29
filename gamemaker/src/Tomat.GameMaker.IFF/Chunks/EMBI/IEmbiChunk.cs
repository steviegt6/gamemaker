using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.EmbeddedImage;

namespace Tomat.GameMaker.IFF.Chunks.EMBI;

/// <summary>
///     The <c>EMBI</c> chunk, which contains embedded images.
/// </summary>
public interface IEmbiChunk : IGameMakerChunk {
    /// <summary>
    ///     The version of the chunk.
    /// </summary>
    /// <remarks>Expected to always be <c>1</c>.</remarks>
    int ChunkVersion { get; set; }

    /// <summary>
    ///     The list of embedded images.
    /// </summary>
    GameMakerList<GameMakerEmbeddedImage> EmbeddedImages { get; set; }
}
