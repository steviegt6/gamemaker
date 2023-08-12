using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.DebugScript;

namespace Tomat.GameMaker.IFF.Chunks.SCPT;

internal sealed class GameMakerDebugScptChunk : AbstractChunk,
                                                IDebugScptChunk {
    public GameMakerPointerList<GameMakerDebugScript> Scripts { get; set; } = null!;

    public GameMakerDebugScptChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Scripts = context.ReadPointerList<GameMakerDebugScript>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Scripts);
    }
}
