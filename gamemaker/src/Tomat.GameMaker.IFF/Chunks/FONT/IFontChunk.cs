using System;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Font;

namespace Tomat.GameMaker.IFF.Chunks.FONT;

public interface IFontChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerFont> Fonts { get; set; }

    Memory<byte>? Padding { get; set; }
}
