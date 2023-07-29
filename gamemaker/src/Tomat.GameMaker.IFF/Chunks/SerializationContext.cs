using System;
using System.Collections.Generic;
using System.Text;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;
using Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents a context for serializing data to a GameMaker IFF file.
/// </summary>
/// <param name="Writer">
///     The writer used to write to a GameMaker IFF file.
/// </param>
/// <param name="IffFile">The GameMaker IFF file being written to.</param>
/// <param name="VersionInfo">
///     The version information of the GameMaker IFF file being read from.
/// </param>
public sealed record SerializationContext(IGameMakerIffWriter Writer, GameMakerIffFile IffFile, GameMakerVersionInfo VersionInfo) : IGameMakerIffWriter {
    public Dictionary<GameMakerVariable, List<(int, GameMakerCodeInstructionVariableType)>> VariableReferences { get; } = new();

    public Dictionary<GameMakerFunctionEntry, List<(int, GameMakerCodeInstructionVariableType)>> FunctionReferences { get; } = new();

#region IGameMakerIffWriter Impl
    public byte[] Data => Writer.Data;

    public int Position {
        get => Writer.Position;

        set => Writer.Position = value;
    }

    public int Length => Writer.Length;

    public Encoding Encoding => Writer.Encoding;

    public Dictionary<IGameMakerSerializable, int> Pointers => Writer.Pointers;

    public Dictionary<IGameMakerSerializable, List<(int, bool)>> PointerReferences => Writer.PointerReferences;

    public void Write(Memory<byte> value) {
        Writer.Write(value);
    }

    public void Write(byte[] value) {
        Writer.Write(value);
    }

    public void Write(char[] value) {
        Writer.Write(value);
    }

    public void Write(byte value) {
        Writer.Write(value);
    }

    public void Write(bool value, bool wide) {
        Writer.Write(value, wide);
    }

    public void Write(short value) {
        Writer.Write(value);
    }

    public void Write(ushort value) {
        Writer.Write(value);
    }

    public void Write(Int24 value) {
        Writer.Write(value);
    }

    public void Write(UInt24 value) {
        Writer.Write(value);
    }

    public void Write(int value) {
        Writer.Write(value);
    }

    public void Write(uint value) {
        Writer.Write(value);
    }

    public void Write(long value) {
        Writer.Write(value);
    }

    public void Write(ulong value) {
        Writer.Write(value);
    }

    public void Write(float value) {
        Writer.Write(value);
    }

    public void Write(double value) {
        Writer.Write(value);
    }

    public void Write<T>(GameMakerPointer<T> ptr, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        Writer.Write(ptr, useTypeOffset);
    }

    public void FinalizePointers() {
        Writer.FinalizePointers();
    }
#endregion

    public void MarkPointerAndWriteObject<T>(GameMakerPointer<T> pointer) where T : IGameMakerSerializable, new() {
        if (!pointer.TryGetObject(out var obj))
            throw new InvalidOperationException("Pointer is null.");

        pointer.WriteObject(this);
        obj.Write(this);
    }

    public void Write<T>(GameMakerList<T> list, GameMakerList<T>.ListWrite? beforeWrite = null, GameMakerList<T>.ListWrite? afterWrite = null, GameMakerList<T>.ListElementWrite? elementWriter = null)
        where T : IGameMakerSerializable, new() {
        list.Write(this, beforeWrite, afterWrite, elementWriter);
    }

    public void Write<T>(
        GameMakerPointerList<T> list,
        GameMakerPointerList<T>.ListWrite? beforeWriter = null,
        GameMakerPointerList<T>.ListWrite? afterWriter = null,
        GameMakerPointerList<T>.ListElementWrite? elementWriter = null,
        GameMakerPointerList<T>.ListElementWrite? elementPointerWriter = null
    ) where T : IGameMakerSerializable, new() {
        list.Write(this, beforeWriter, afterWriter, elementWriter, elementPointerWriter);
    }

    public void Write<T>(
        GameMakerRemotePointerList<T> list,
        GameMakerRemotePointerList<T>.ListWrite? beforeWriter = null,
        GameMakerRemotePointerList<T>.ListWrite? afterWriter = null,
        GameMakerRemotePointerList<T>.ListElementWrite? elementPointerWriter = null
    ) where T : IGameMakerSerializable, new() {
        list.Write(this, beforeWriter, afterWriter, elementPointerWriter);
    }

    public void WriteNullTerminatedString(string value, Encoding encoding) {
        var bytes = encoding.GetBytes(value);
        Write(bytes);
        Write((byte)0);
    }
};
