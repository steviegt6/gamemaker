using System;
using Tomat.GameMaker.IFF.DataTypes.Models.Font;

namespace Tomat.GameMaker.IFF.Chunks.FONT;

/// <summary>
///     The <c>FONT</c> chunk, which contains fonts.
/// </summary>
public interface IFontChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of fonts.
    /// </summary>
    GameMakerPointerList<GameMakerFont> Fonts { get; set; }

    /// <summary>
    ///     The padding of the chunk.
    /// </summary>
    Memory<byte>? Padding { get; set; }
}
