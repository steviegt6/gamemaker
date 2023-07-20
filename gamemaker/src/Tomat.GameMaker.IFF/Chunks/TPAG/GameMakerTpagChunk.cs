using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.Chunks.TPAG;

public sealed class GameMakerTpagChunk : AbstractChunk {
    public const string NAME = "TPAG";

    public GameMakerPointerList<GameMakerTextureItem> TexturePageItems { get; set; } = null!;

    public GameMakerTpagChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        TexturePageItems = context.ReadPointerList<GameMakerTextureItem>();
    }

    public override void Write(SerializationContext context) {
        context.Write(TexturePageItems);
    }
}
