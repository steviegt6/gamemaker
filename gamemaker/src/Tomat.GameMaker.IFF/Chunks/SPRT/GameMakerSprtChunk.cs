using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Sprite;

namespace Tomat.GameMaker.IFF.Chunks.SPRT;

internal sealed class GameMakerSprtChunk : AbstractChunk,
                                           ISprtChunk {
    public const string NAME = "SPRT";

    public GameMakerPointerList<GameMakerSprite> Sprites { get; set; } = null!;

    public GameMakerSprtChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Sprites = context.ReadPointerList<GameMakerSprite>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Sprites);
    }
}
