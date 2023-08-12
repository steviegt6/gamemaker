using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Extension;

namespace Tomat.GameMaker.IFF.Chunks.EXTN;

/// <summary>
///     The <c>EXTN</c> chunk, which contains extensions.
/// </summary>
public interface IExtnChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of extensions.
    /// </summary>
    GameMakerPointerList<GameMakerExtension> Extensions { get; set; }
}
