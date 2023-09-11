using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.Chunks.TPAG;

internal sealed class GameMakerTpagChunk : AbstractChunk,
                                           ITpagChunk {
    public const string NAME = "TPAG";

    public GameMakerPointerList<GameMakerTextureItem> TexturePageItems { get; set; } = null!;

    public GameMakerTpagChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        TexturePageItems = context.ReadPointerList<GameMakerTextureItem>();
    }

    public override void Write(SerializationContext context) {
        context.Write(TexturePageItems);
    }
}
