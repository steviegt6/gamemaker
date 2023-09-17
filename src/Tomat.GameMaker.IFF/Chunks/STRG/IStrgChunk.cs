using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks.STRG;

public interface IStrgChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerString> Strings { get; set; }
}
