using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Extension;

namespace Tomat.GameMaker.IFF.Chunks.EXTN;

public interface IExtnChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerExtension> Extensions { get; set; }
}
