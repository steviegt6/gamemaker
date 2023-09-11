using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.Chunks.TXTR;

public interface ITxtrChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerTexturePage> TexturePages { get; set; }
}
