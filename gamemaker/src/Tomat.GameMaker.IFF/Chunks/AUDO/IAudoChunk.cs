using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Audio;

namespace Tomat.GameMaker.IFF.Chunks.AUDO;

public interface IAudoChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerAudio> Audio { get; set; }
}
