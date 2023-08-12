using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.FORM;

/// <summary>
///     The <c>FORM</c> chunk, which contains other chunks.
/// </summary>
public interface IFormChunk : IGameMakerChunk {
    /// <summary>
    ///     The map of chunk names to their instances.
    /// </summary>
    Dictionary<string, IGameMakerChunk> Chunks { get; set; }
}
