using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Script;

namespace Tomat.GameMaker.IFF.Chunks.SCPT;

internal sealed class GameMakerScptChunk : AbstractChunk,
                                           IScptChunk {
    public const string NAME = "SCPT";

    public GameMakerPointerList<GameMakerScript> Scripts { get; set; } = null!;

    public GameMakerScptChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Scripts = context.ReadPointerList<GameMakerScript>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Scripts);
    }
}
