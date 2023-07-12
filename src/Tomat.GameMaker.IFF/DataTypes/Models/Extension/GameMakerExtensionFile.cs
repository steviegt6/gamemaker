using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Extension;

public sealed class GameMakerExtensionFile : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> FileName { get; set; }

    public GameMakerPointer<GameMakerString> FinalFunction { get; set; }

    public GameMakerPointer<GameMakerString> InitFunction { get; set; }

    public GameMakerExtensionKind ExtensionKind { get; set; }

    public GameMakerPointerList<GameMakerExtensionFunction> Functions { get; set; } = null!;

    public void Read(DeserializationContext context) {
        FileName = context.ReadPointerAndObject<GameMakerString>();
        FinalFunction = context.ReadPointerAndObject<GameMakerString>();
        InitFunction = context.ReadPointerAndObject<GameMakerString>();
        ExtensionKind = (GameMakerExtensionKind)context.ReadUInt32();
        Functions = context.ReadPointerList<GameMakerExtensionFunction>();
    }

    public void Write(SerializationContext context) {
        context.Write(FileName);
        context.Write(FinalFunction);
        context.Write(InitFunction);
        context.Write((uint)ExtensionKind);
        context.Write(Functions);
    }
}
