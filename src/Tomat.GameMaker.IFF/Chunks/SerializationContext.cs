using System;
using System.Collections.Generic;
using System.Text;
using Tomat.GameMaker.IFF.DataTypes;
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
        if (pointer.IsNull)
            throw new InvalidOperationException("Pointer is null.");

        if (pointer.Object is null)
            throw new InvalidOperationException("Pointer has not been read or its object was incorrectly set to null.");

        pointer.WriteObject(this);
        pointer.ExpectObject().Write(this);
    }
};
