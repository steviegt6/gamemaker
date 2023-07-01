using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerExtensionFile : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> FileName { get; set; }

    public GameMakerPointer<GameMakerString> FinalFunction { get; set; }

    public GameMakerPointer<GameMakerString> InitFunction { get; set; }

    public GameMakerExtensionKind ExtensionKind { get; set; }

    public GameMakerPointerList<GameMakerExtensionFunction>? Functions { get; set; }

    public void Read(DeserializationContext context) {
        FileName = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
        FinalFunction = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
        InitFunction = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
        ExtensionKind = (GameMakerExtensionKind)context.Reader.ReadUInt32();
        Functions = new GameMakerPointerList<GameMakerExtensionFunction>();
        Functions.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(FileName);
        context.Writer.Write(FinalFunction);
        context.Writer.Write(InitFunction);
        context.Writer.Write((uint)ExtensionKind);
        Functions!.Write(context);
    }
}
