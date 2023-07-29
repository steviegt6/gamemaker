using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.DebugFunctionInfo;

public sealed class GameMakerDebugFunctionInfo : IGameMakerSerializable {
    public int Index { get; set; }

    public GameMakerPointer<GameMakerString> Undecorated { get; set; }

    public int Length { get; set; }

    public List<GameMakerPointer<GameMakerString>> ArgumentNames { get; set; }

    public List<GameMakerPointer<GameMakerString>> LocalNames { get; set; }

    public void Read(DeserializationContext context) {
        Index = context.ReadInt32();
        Undecorated = context.ReadPointerAndObject<GameMakerString>();
        Length = context.ReadInt32();
        var argumentCount = context.ReadInt32();
        ArgumentNames = new List<GameMakerPointer<GameMakerString>>(argumentCount);
        for (var i = 0; i < argumentCount; i++)
            ArgumentNames.Add(context.ReadPointerAndObject<GameMakerString>());
        var localCount = context.ReadInt32();
        LocalNames = new List<GameMakerPointer<GameMakerString>>(localCount);
        for (var i = 0; i < localCount; i++)
            LocalNames.Add(context.ReadPointerAndObject<GameMakerString>());
    }

    public void Write(SerializationContext context) {
        context.Write(Index);
        context.Write(Undecorated);
        context.Write(Length);
        context.Write(ArgumentNames.Count);
        foreach (var argumentName in ArgumentNames)
            context.Write(argumentName);
        context.Write(LocalNames.Count);
        foreach (var localName in LocalNames)
            context.Write(localName);
    }
}
