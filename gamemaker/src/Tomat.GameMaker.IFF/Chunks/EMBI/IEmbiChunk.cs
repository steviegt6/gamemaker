using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.EmbeddedImage;

namespace Tomat.GameMaker.IFF.Chunks.EMBI;

public interface IEmbiChunk : IGameMakerChunk {
    int ChunkVersion { get; set; }

    GameMakerList<GameMakerEmbeddedImage> EmbeddedImages { get; set; }
}
