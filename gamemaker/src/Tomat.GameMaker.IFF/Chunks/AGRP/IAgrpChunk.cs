using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Audio;

namespace Tomat.GameMaker.IFF.Chunks.AGRP;

public interface IAgrpChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerAudioGroup> AudioGroups { get; set; }
}
