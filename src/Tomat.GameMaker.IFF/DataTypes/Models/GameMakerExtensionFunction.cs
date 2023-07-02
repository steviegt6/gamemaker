using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerExtensionFunction : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int Id { get; set; }

    public int FunctionKind { get; set; }

    public GameMakerExtensionValueType ReturnType { get; set; }

    public GameMakerPointer<GameMakerString> ExternalName { get; set; }

    public List<GameMakerExtensionValueType>? ArgumentTypes { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Id = context.Reader.ReadInt32();
        FunctionKind = context.Reader.ReadInt32();
        ReturnType = (GameMakerExtensionValueType)context.Reader.ReadInt32();
        ExternalName = context.ReadPointerAndObject<GameMakerString>();

        ArgumentTypes = new List<GameMakerExtensionValueType>();
        var argumentCount = context.Reader.ReadInt32();
        for (var i = 0; i < argumentCount; i++)
            ArgumentTypes.Add((GameMakerExtensionValueType)context.Reader.ReadUInt32());
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Id);
        context.Writer.Write(FunctionKind);
        context.Writer.Write((int)ReturnType);
        context.Writer.Write(ExternalName);

        context.Writer.Write(ArgumentTypes!.Count);
        foreach (var argumentType in ArgumentTypes)
            context.Writer.Write((uint)argumentType);
    }
}
