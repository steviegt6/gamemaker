using Tomat.GameMaker.IFF.DataTypes.Models.Path;

namespace Tomat.GameMaker.IFF.Chunks.PATH;

internal sealed class GameMakerPathChunk : AbstractChunk,
                                           IPathChunk {
    public const string NAME = "PATH";

    public GameMakerPointerList<GameMakerPath> Paths { get; set; } = null!;

    public GameMakerPathChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Paths = context.ReadPointerList<GameMakerPath>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Paths);
    }
}
