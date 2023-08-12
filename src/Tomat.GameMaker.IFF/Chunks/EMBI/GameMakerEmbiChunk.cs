using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.EmbeddedImage;

namespace Tomat.GameMaker.IFF.Chunks.EMBI;

internal sealed class GameMakerEmbiChunk : AbstractChunk,
                                           IEmbiChunk {
    public const string NAME = "EMBI";

    public int ChunkVersion { get; set; }

    public GameMakerList<GameMakerEmbeddedImage> EmbeddedImages { get; set; } = null!;

    public GameMakerEmbiChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        ChunkVersion = context.ReadInt32();
        if (ChunkVersion != 1)
            throw new InvalidDataException("Expected chunk version 1, got " + ChunkVersion);

        EmbeddedImages = context.ReadList<GameMakerEmbeddedImage>();
    }

    public override void Write(SerializationContext context) {
        if (ChunkVersion != 1)
            throw new InvalidDataException("Expected chunk version 1, got " + ChunkVersion);

        context.Write(ChunkVersion);
        context.Write(EmbeddedImages);
    }
}
