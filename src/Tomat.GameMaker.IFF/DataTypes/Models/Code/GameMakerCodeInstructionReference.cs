using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Code; 

public sealed class GameMakerCodeInstructionReference<T> : IGameMakerSerializable {
    public int NextOccurence { get; set; }
    
    public GameMakerCodeInstructionVariableType VariableType { get; set; }

    public T? Target { get; set; }

    public GameMakerCodeInstructionReference() { }

    public GameMakerCodeInstructionReference(T target) {
        Target = target;
    }

    public GameMakerCodeInstructionReference(T target, GameMakerCodeInstructionVariableType variableType) {
        Target = target;
        VariableType = variableType;
    }

    public GameMakerCodeInstructionReference(int value) {
        NextOccurence = value & 0x07FFFFFF;
        VariableType = (GameMakerCodeInstructionVariableType)((value >> 24) & 0xF8);
    }

    public void Read(DeserializationContext context) {
        var value = context.ReadInt32();
        NextOccurence = value & 0x07FFFFFF;
        VariableType = (GameMakerCodeInstructionVariableType)((value >> 24) & 0xF8);
    }

    public void Write(SerializationContext context) {
        context.Write((Int24)0);
        context.Write((byte)VariableType);
    }
}
