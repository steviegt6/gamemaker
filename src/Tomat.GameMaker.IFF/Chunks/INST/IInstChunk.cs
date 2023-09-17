using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks.INST;

public interface IInstChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerString> InstanceNames { get; set; }
}
