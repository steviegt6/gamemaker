using System;
using System.Collections.Generic;
using System.Linq;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Code; 

public sealed class GameMakerCodeBytecode : IGameMakerSerializable {
    public GameMakerCode Parent { get; }

    public List<GameMakerCodeInstruction> Instructions { get; } = new(64);

    public int Length => Instructions.Sum(x => x.Length);

    public GameMakerCodeBytecode(GameMakerCode parent) {
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
    
    void IGameMakerSerializable.Read(DeserializationContext context) {
        throw new NotImplementedException("Attempted to read a bytecode entry without a given length.");
    }
}
