using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Background;

namespace Tomat.GameMaker.IFF.Chunks.BGND;

/// <summary>
///     The <c>BGND</c> chunk, which contains backgrounds.
/// </summary>
public interface IBgndChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of backgrounds.
    /// </summary>
    GameMakerPointerList<GameMakerBackground> Backgrounds { get; set; }
}
