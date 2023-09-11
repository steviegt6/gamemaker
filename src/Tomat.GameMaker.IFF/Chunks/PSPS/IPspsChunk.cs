using System;

namespace Tomat.GameMaker.IFF.Chunks.PSPS;

// chunk PSPS {
//     byte[16] passcode;
// }

public interface IPspsChunk : IGameMakerChunk {
    Memory<byte> Passcode { get; set; }
}
