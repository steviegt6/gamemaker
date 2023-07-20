using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Path;

namespace Tomat.GameMaker.IFF.Chunks.PATH;

public sealed class GameMakerPathChunk : AbstractChunk {
    public const string NAME = "PATH";

    public GameMakerPointerList<GameMakerPath> Paths { get; set; } = null!;

    public GameMakerPathChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Paths = context.ReadPointerList<GameMakerPath>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Paths);
    }
}
