using System;
using System.Collections.Generic;
using System.Text;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents a context for serializing data to a GameMaker IFF file.
/// </summary>
/// <param name="Reader">
///     The reader used to read from a GameMaker IFF file.
/// </param>
/// <param name="IffFile">The GameMaker IFF file being read from.</param>
/// <param name="VersionInfo">
///     The version information of the GameMaker IFF file being read from.
/// </param>
public sealed record DeserializationContext(IGameMakerIffReader Reader, GameMakerIffFile IffFile, GameMakerVersionInfo VersionInfo) : IGameMakerIffReader {
    public Dictionary<int, GameMakerCodeInstruction> Instructions { get; } = new();

#region IGameMakerIffReader Impl
    public byte[] Data => Reader.Data;

    public int Position {
        get => Reader.Position;
        set => Reader.Position = value;
    }

    public int Length => Reader.Length;

    public Encoding Encoding => Reader.Encoding;

    public Dictionary<int, IGameMakerSerializable> Pointers => Reader.Pointers;

    public Memory<byte> ReadBytes(int count) {
        return Reader.ReadBytes(count);
    }

    public char[] ReadChars(int count) {
        return Reader.ReadChars(count);
    }

    public byte ReadByte() {
        return Reader.ReadByte();
    }

    public bool ReadBoolean(bool wide) {
        return Reader.ReadBoolean(wide);
    }

    public short ReadInt16() {
        return Reader.ReadInt16();
    }

    public ushort ReadUInt16() {
        return Reader.ReadUInt16();
    }

    public Int24 ReadInt24() {
        return Reader.ReadInt24();
    }

    public UInt24 ReadUInt24() {
        return Reader.ReadUInt24();
    }

    public int ReadInt32() {
        return Reader.ReadInt32();
    }

    public uint ReadUInt32() {
        return Reader.ReadUInt32();
    }

    public long ReadInt64() {
        return Reader.ReadInt64();
    }

    public ulong ReadUInt64() {
        return Reader.ReadUInt64();
    }

    public float ReadSingle() {
        return Reader.ReadSingle();
    }

    public double ReadDouble() {
        return Reader.ReadDouble();
    }

    public GameMakerPointer<TInterface> ReadPointer<TInterface, TImplementation>(
        int addr,
        bool useTypeOffset = true
    ) where TInterface : IGameMakerSerializable
      where TImplementation : TInterface, new() {
        return Reader.ReadPointer<TInterface, TImplementation>(addr, useTypeOffset);
    }
#endregion

    public GameMakerPointer<TInterface> ReadPointerAndObject<TInterface, TImplementation>(
        int addr,
        bool returnAfter = true,
        bool useTypeOffset = true
    ) where TInterface : IGameMakerSerializable
      where TImplementation : TInterface, new() {
        var ptr = ReadPointer<TInterface, TImplementation>(addr, useTypeOffset);
        ptr.ReadPointerObject<TImplementation>(this, returnAfter);
        return ptr;
    }

    public GameMakerPointer<TInterface> ReadPointerAndObject<TInterface, TImplementation>(bool returnAfter = true, bool useTypeOffset = true) where TInterface : IGameMakerSerializable
                                                                                                                                              where TImplementation : TInterface, new() {
        return ReadPointerAndObject<TInterface, TImplementation>(ReadInt32(), returnAfter, useTypeOffset);
    }

    public string ReadNullTerminatedString(Encoding encoding) {
        var startPos = Position;

        while (ReadByte() != 0) { }

        var endPos = Position;
        Position = startPos;
        var str = encoding.GetString(ReadBytes(endPos - startPos - 1).Span);
        Position++; // Skip the null terminator.
        return str;
    }
}
