using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sprite;

public sealed class GameMakerSpriteSequenceReference : IGameMakerSerializable {
    public int Version { get; set; }

    public GameMakerSequence? Sequence { get; set; }

    public void Read(DeserializationContext context) {
        Version = context.Reader.ReadInt32();
        if (Version != 1)
            throw new InvalidDataException($"Expected version 1, got {Version}.");

        Sequence = new GameMakerSequence();
        Sequence.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Version);
        Sequence!.Write(context);
    }
}
