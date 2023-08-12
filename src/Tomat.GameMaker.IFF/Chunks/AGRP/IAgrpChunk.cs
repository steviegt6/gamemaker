using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Audio;

namespace Tomat.GameMaker.IFF.Chunks.AGRP;

/// <summary>
///     The <c>AGRP</c> chunk, which contains audio groups.
/// </summary>
public interface IAgrpChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of audio groups.
    /// </summary>
    GameMakerPointerList<GameMakerAudioGroup> AudioGroups { get; set; }
}
