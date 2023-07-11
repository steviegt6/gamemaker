using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.EmbeddedImage;

namespace Tomat.GameMaker.IFF.Chunks.EMBI;

public sealed class GameMakerEmbiChunk : AbstractChunk {
    public const string NAME = "EMBI";

    public GameMakerList<GameMakerEmbeddedImage>? EmbeddedImages { get; set; }

    public GameMakerEmbiChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        var chunkVersion = context.Reader.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException("Expected chunk version 1, got " + chunkVersion);

        EmbeddedImages = new GameMakerList<GameMakerEmbeddedImage>();
        EmbeddedImages.Read(context);
    }

    public override void Write(SerializationContext context) {
        context.Writer.Write(1);
        EmbeddedImages?.Write(context);
    }
}
