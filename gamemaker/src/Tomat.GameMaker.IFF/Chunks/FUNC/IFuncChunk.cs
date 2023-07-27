using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;
using Tomat.GameMaker.IFF.DataTypes.Models.Local;

namespace Tomat.GameMaker.IFF.Chunks.FUNC;

public interface IFuncChunk : IGameMakerChunk {
    GameMakerList<GameMakerFunctionEntry> FunctionEntries { get; set; }

    GameMakerList<GameMakerLocalsEntry> Locals { get; set; }
}
