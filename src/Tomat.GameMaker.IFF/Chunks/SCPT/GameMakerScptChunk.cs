using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Script;

namespace Tomat.GameMaker.IFF.Chunks.SCPT;

public sealed class GameMakerScptChunk : AbstractChunk {
    public const string NAME = "SCPT";

    public GameMakerPointerList<GameMakerScript>? Scripts { get; set; }

    public GameMakerScptChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Scripts = new GameMakerPointerList<GameMakerScript>();
        Scripts.Read(context);
    }

    public override void Write(SerializationContext context) {
        Scripts!.Write(context);
    }
}
