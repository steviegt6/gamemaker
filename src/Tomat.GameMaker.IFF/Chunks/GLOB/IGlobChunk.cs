using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GLOB;

public interface IGlobChunk : IGameMakerChunk {
    List<int> GlobalCodeIds { get; }
}
