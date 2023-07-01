using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerExtensionOption : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Value { get; set; }

    public GameMakerExtensionOptionKind OptionKind { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Value = context.ReadPointerAndObject<GameMakerString>();
        OptionKind = (GameMakerExtensionOptionKind)context.Reader.ReadUInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Value);
        context.Writer.Write((int)OptionKind);
    }
}
