using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Extension;

public sealed class GameMakerExtensionFunction : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int Id { get; set; }

    public int FunctionKind { get; set; }

    public GameMakerExtensionValueType ReturnType { get; set; }

    public GameMakerPointer<GameMakerString> ExternalName { get; set; }

    public List<GameMakerExtensionValueType>? ArgumentTypes { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Id = context.ReadInt32();
        FunctionKind = context.ReadInt32();
        ReturnType = (GameMakerExtensionValueType)context.ReadUInt32();
        ExternalName = context.ReadPointerAndObject<GameMakerString>();

        ArgumentTypes = new List<GameMakerExtensionValueType>();
        var argumentCount = context.ReadInt32();
        for (var i = 0; i < argumentCount; i++)
            ArgumentTypes.Add((GameMakerExtensionValueType)context.ReadUInt32());
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Id);
        context.Write(FunctionKind);
        context.Write((uint)ReturnType);
        context.Write(ExternalName);

        context.Write(ArgumentTypes!.Count);
        foreach (var argumentType in ArgumentTypes)
            context.Write((uint)argumentType);
    }
}
