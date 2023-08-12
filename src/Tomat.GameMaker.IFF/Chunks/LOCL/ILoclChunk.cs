using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Local;

namespace Tomat.GameMaker.IFF.Chunks.LOCL;

public interface ILoclChunk {
    GameMakerPointerList<GameMakerLocalsEntry> Locals { get; set; }
}
