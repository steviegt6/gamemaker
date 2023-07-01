using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerAudioGroup : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
    }
}
