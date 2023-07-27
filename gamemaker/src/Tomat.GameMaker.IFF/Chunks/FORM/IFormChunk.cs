using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.FORM;

public interface IFormChunk : IGameMakerChunk {
    Dictionary<string, IGameMakerChunk> Chunks { get; set; }
}
