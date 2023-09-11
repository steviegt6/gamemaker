using Tomat.GameMaker.IFF.DataTypes.Models.Local;

namespace Tomat.GameMaker.IFF.Chunks.LOCL;

internal sealed class GameMakerLoclChunk : AbstractChunk,
                                           ILoclChunk {
    public const string NAME = "LOCL";

    public GameMakerPointerList<GameMakerLocalsEntry> Locals { get; set; } = null!;

    public GameMakerLoclChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Locals = context.ReadPointerList<GameMakerLocalsEntry>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Locals);
    }
}
