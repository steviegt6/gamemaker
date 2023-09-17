using System;
using System.Collections.Generic;
using System.Linq;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Code;

public interface ICodeBytecode : IGameMakerSerializable {
    ICode Parent { get; set; }

    List<ICodeInstruction> Instructions { get; set; }

    int Length { get; }

    void Read(DeserializationContext context, int length);

    void IGameMakerSerializable.Read(DeserializationContext context) {
        throw new InvalidOperationException("Attempted to read a bytecode entry without a given length.");
    }
}

internal sealed class GameMakerCodeBytecode : ICodeBytecode {
    public ICode Parent { get; set; }

    public List<ICodeInstruction> Instructions { get; set; } = new(64);

    public int Length => Instructions.Sum(x => x.Length);

    public GameMakerCodeBytecode(ICode parent) {
        Parent = parent;
    }

    public void Read(DeserializationContext context, int length) {
        var begin = context.Position;
        var end = begin + length;

        while (context.Position < end) {
            var instruction = new GameMakerCodeInstruction(context.Position - begin);
            instruction.Read(context);
            Instructions.Add(instruction);
        }
    }

    public void Write(SerializationContext context) {
        foreach (var instruction in Instructions)
            instruction.Write(context);
    }
}
