using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Sound;

namespace Tomat.GameMaker.IFF.Chunks.SOND;

public sealed class GameMakerSondChunk : AbstractChunk {
    public const string NAME = "SOND";

    public GameMakerPointerList<GameMakerSound> Sounds { get; set; } = null!;

    public GameMakerSondChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Sounds = context.ReadPointerList<GameMakerSound>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Sounds);
    }
}
