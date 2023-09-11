using Tomat.GameMaker.IFF.DataTypes.Models.Sound;

namespace Tomat.GameMaker.IFF.Chunks.SOND;

internal sealed class GameMakerSondChunk : AbstractChunk,
                                           ISondChunk {
    public const string NAME = "SOND";

    public GameMakerPointerList<GameMakerSound> Sounds { get; set; } = null!;

    public GameMakerSondChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Sounds = context.ReadPointerList<GameMakerSound>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Sounds);
    }
}
