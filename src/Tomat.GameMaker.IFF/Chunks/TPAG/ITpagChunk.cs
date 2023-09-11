using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.Chunks.TPAG;

public interface ITpagChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerTextureItem> TexturePageItems { get; set; }
}
