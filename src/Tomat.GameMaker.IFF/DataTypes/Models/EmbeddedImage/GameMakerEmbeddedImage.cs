using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.DataTypes.Models.EmbeddedImage;

public sealed class GameMakerEmbeddedImage : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerTextureItem> TextureItem { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        TextureItem = context.ReadPointerAndObject<GameMakerTextureItem>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(TextureItem);
    }
}
