using System;

namespace Tomat.GameMaker.IFF.Chunks.PSPS;

internal sealed class GameMakerPspsChunk : AbstractChunk,
                                           IPspsChunk {
    public const string NAME = "PSPS";
    private const int passcode_length = 16;

    public Memory<byte> Passcode { get; set; } = null!;

    public GameMakerPspsChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Passcode = context.ReadBytes(passcode_length);
    }

    public override void Write(SerializationContext context) {
        if (Passcode.Length != passcode_length)
            throw new InvalidOperationException($"Passcode must be {passcode_length} bytes long.");

        context.Write(Passcode);
    }
}
