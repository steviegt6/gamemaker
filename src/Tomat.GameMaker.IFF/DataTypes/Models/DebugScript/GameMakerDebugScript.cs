namespace Tomat.GameMaker.IFF.DataTypes.Models.DebugScript;

public sealed class GameMakerDebugScript : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Source { get; set; }

    public void Read(DeserializationContext context) {
        Source = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Write(Source);
    }
}
