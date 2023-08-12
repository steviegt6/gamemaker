using System;

namespace Tomat.GameMaker.IFF.Chunks.PSPS;

public interface IPspsChunk : IGameMakerChunk {
    Memory<byte> Passcode { get; set; }
}
