namespace Tomat.GameMaker.IFF.DataTypes.Models.Extension;

public sealed class GameMakerExtensionOption : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Value { get; set; }

    public GameMakerExtensionOptionKind OptionKind { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Value = context.ReadPointerAndObject<GameMakerString>();
        OptionKind = (GameMakerExtensionOptionKind)context.ReadUInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Value);
        context.Write((int)OptionKind);
    }
}
