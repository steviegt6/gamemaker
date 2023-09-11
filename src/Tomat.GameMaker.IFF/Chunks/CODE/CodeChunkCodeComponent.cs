using Tomat.GameMaker.IFF.DataTypes.Models.Code;

namespace Tomat.GameMaker.IFF.Chunks.CODE;

/// <summary>
///     A component in the <c>CODE</c> chunk, containing the code.
/// </summary>
public interface ICodeChunkCodeComponent {
    /// <summary>
    ///     The list of code.
    /// </summary>
    public GameMakerPointerList<GameMakerCode> Code { get; set; }
}

internal sealed class CodeChunkCodeComponent : ICodeChunkCodeComponent {
    public required GameMakerPointerList<GameMakerCode> Code { get; set; }
}
