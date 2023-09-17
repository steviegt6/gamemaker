using System;
using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Code;

public interface ICode : IGameMakerSerializable {
    GameMakerPointer<IString> Name { get; set; }

    int Length { get; set; }

    short LocalsCount { get; set; }

    short ArgumentsCount { get; set; }

    byte Flags { get; set; }

    ICodeBytecode Bytecode { get; set; }

    ICode? Parent { get; set; }

    List<ICode> Children { get; set; }
}

internal sealed class GameMakerCode : ICode {
    public GameMakerPointer<IString> Name { get; set; }

    public int Length { get; set; }

    public short LocalsCount { get; set; }

    public short ArgumentsCount { get; set; }

    public byte Flags { get; set; }

    public int BytecodeOffset { get; set; }

    public ICodeBytecode Bytecode { get; set; } = null!;

    public ICode? Parent { get; set; }

    public List<ICode> Children { get; set; } = new();

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<IString, GameMakerString>();
        Length = context.ReadInt32();

        // Early exit, if we're on <= 14.
        if (context.VersionInfo.FormatId <= 14) {
            Bytecode = new GameMakerCodeBytecode(this);
            Bytecode.Read(context, Length);
            return;
        }

        LocalsCount = context.ReadInt16();
        var v = context.ReadInt16();
        ArgumentsCount = (short)(v & 0b1111111111111);
        Flags = (byte)(v >> 13);
        var relativeBytecodeAddress = context.ReadInt32();
        var absoluteBytecodeAddress = (context.Position - 4) + relativeBytecodeAddress;
        var childCandidate = false;

        if (context.Pointers.TryGetValue(absoluteBytecodeAddress, out var ptr)) {
            if (ptr is GameMakerCodeBytecode bytecode) {
                Bytecode = bytecode;
                childCandidate = true;
            }
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Bytecode is null) {
            Bytecode = new GameMakerCodeBytecode(this);
            if (Length != 0)
                context.Pointers[absoluteBytecodeAddress] = Bytecode;

            var returnTo = context.Position;
            context.Position = absoluteBytecodeAddress;

            Bytecode.Read(context, Length);
            context.Position = returnTo;
        }

        BytecodeOffset = context.ReadInt32();

        if (childCandidate && Length != 0 && BytecodeOffset != 0) {
            Parent = Bytecode.Parent;
            Bytecode.Parent.Children.Add(this);
        }
    }

    public void Write(SerializationContext context) {
        if (Bytecode is null)
            throw new InvalidOperationException("Attempted to write a bytecode entry without a given length.");

        context.Write(Name);
        Length = Bytecode.Length * 4;
        context.Write(Length);

        if (context.VersionInfo.FormatId <= 14) {
            Bytecode.Write(context);
        }
        else {
            context.Write(LocalsCount);
            context.Write((short)((int)ArgumentsCount | ((int)Flags << 13)));
            context.Write(context.Pointers[Bytecode] - context.Position);
            context.Write(BytecodeOffset);
        }
    }
}
