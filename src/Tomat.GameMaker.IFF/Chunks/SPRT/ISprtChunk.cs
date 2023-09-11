using Tomat.GameMaker.IFF.DataTypes.Models.Sprite;

namespace Tomat.GameMaker.IFF.Chunks.SPRT;

public interface ISprtChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerSprite> Sprites { get; set; }
}
