using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks.INST;

internal sealed class GameMakerInstChunk : AbstractChunk,
                                           IInstChunk {
    public const string NAME = "INST";

    public GameMakerPointerList<GameMakerString> InstanceNames { get; set; } = null!;

    public GameMakerInstChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        InstanceNames = context.ReadPointerList<GameMakerString>();
    }

    public override void Write(SerializationContext context) {
        context.Write(InstanceNames);
    }
}
