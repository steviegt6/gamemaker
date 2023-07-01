using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks.SOND; 

public sealed class GameMakerSondChunk : AbstractChunk {
    public const string NAME = "SOND";

    public GameMakerPointerList<GameMakerSound>? Sounds { get; set; }

    public GameMakerSondChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Sounds = new GameMakerPointerList<GameMakerSound>();
        Sounds.Read(context);
    }

    public override void Write(SerializationContext context) {
        Sounds!.Write(context);
    }
}
