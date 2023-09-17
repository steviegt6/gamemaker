namespace Tomat.GameMaker.IFF.DataTypes.Models.FilterEffect; 

public sealed class GameMakerFilterEffect : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }
    
    public GameMakerPointer<GameMakerString> Value { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Value = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Value);
    }
}
