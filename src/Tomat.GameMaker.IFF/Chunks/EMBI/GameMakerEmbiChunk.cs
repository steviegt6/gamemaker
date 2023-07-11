using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.EmbeddedImage;

namespace Tomat.GameMaker.IFF.Chunks.EMBI;

public sealed class GameMakerEmbiChunk : AbstractChunk {
    public const string NAME = "EMBI";

    public GameMakerList<GameMakerEmbeddedImage> EmbeddedImages { get; set; } = null!;

    public GameMakerEmbiChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        var chunkVersion = context.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException("Expected chunk version 1, got " + chunkVersion);

        EmbeddedImages = context.ReadList<GameMakerEmbeddedImage>();
    }

    public override void Write(SerializationContext context) {
        context.Write(1);
        context.Write(EmbeddedImages);
    }
}
