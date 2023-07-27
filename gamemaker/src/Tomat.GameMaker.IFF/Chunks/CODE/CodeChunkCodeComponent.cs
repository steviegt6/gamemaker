using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;

namespace Tomat.GameMaker.IFF.Chunks.CODE;

public interface ICodeChunkCodeComponent {
    public GameMakerPointerList<GameMakerCode> Code { get; set; }
}

internal sealed class CodeChunkCodeComponent : ICodeChunkCodeComponent {
    public required GameMakerPointerList<GameMakerCode> Code { get; set; }
}
