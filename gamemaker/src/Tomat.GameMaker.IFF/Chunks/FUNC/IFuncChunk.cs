using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;
using Tomat.GameMaker.IFF.DataTypes.Models.Local;

namespace Tomat.GameMaker.IFF.Chunks.FUNC;

/// <summary>
///     The <c>FUNC</c> chunk, which contains function entries and code locals.
/// </summary>
public interface IFuncChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of function entries.
    /// </summary>
    GameMakerList<GameMakerFunctionEntry> FunctionEntries { get; set; }

    /// <summary>
    ///     The list of code locals.
    /// </summary>
    GameMakerList<GameMakerLocalsEntry> Locals { get; set; }
}
