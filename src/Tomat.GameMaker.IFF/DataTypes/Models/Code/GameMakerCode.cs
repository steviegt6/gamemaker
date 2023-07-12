using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Code;

public sealed class GameMakerCode : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int Length { get; set; }

    public short LocalsCount { get; set; }

    public short ArgumentsCount { get; set; }

    public byte Flags { get; set; }

    public int BytecodeOffset { get; set; }

    public GameMakerCodeBytecode? BytecodeEntry { get; set; }

    public GameMakerCode? ParentEntry { get; set; }

    public List<GameMakerCode> Children { get; } = new();

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Length = context.ReadInt32();

        // Early exit, if we're on <= 14.
        if (context.VersionInfo.FormatId <= 14) {
            BytecodeEntry = new GameMakerCodeBytecode(this);
            BytecodeEntry.Read(context, Length);
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
                BytecodeEntry = bytecode;
                childCandidate = true;
            }
        }

        if (BytecodeEntry is null) {
            BytecodeEntry = new GameMakerCodeBytecode(this);
            if (Length != 0)
                context.Pointers[absoluteBytecodeAddress] = BytecodeEntry;

            var returnTo = context.Position;
            context.Position = absoluteBytecodeAddress;

            BytecodeEntry.Read(context, Length);
            context.Position = returnTo;
        }

        BytecodeOffset = context.ReadInt32();

        if (childCandidate && Length != 0 && BytecodeOffset != 0) {
            ParentEntry = BytecodeEntry.Parent;
            BytecodeEntry.Parent.Children.Add(this);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        if (BytecodeEntry is not null)
            Length = BytecodeEntry.Length * 4;
        context.Write(Length);

        if (BytecodeEntry is null)
            throw new InvalidOperationException("Attempted to write a bytecode entry without a given length.");

        if (context.VersionInfo.FormatId <= 14) {
            BytecodeEntry.Write(context);
        }
        else {
            context.Write(LocalsCount);
            context.Write((short)((int)ArgumentsCount | ((int)Flags << 13)));
            context.Write(context.Pointers[BytecodeEntry] - context.Position);
            context.Write(BytecodeOffset);
        }
    }
}
