using Tomat.GameMaker.IFF.DataTypes.Models.Audio;

namespace Tomat.GameMaker.IFF.Chunks.AUDO;

/// <summary>
///     The <c>AUDO</c> chunk, which contains audio.
/// </summary>
public interface IAudoChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of audio.
    /// </summary>
    GameMakerPointerList<GameMakerAudio> Audio { get; set; }
}
