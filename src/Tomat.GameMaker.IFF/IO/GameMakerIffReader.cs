using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.IO;

/// <summary>
///     Responsible for reading data from a GameMaker IFF file.
/// </summary>
public sealed class GameMakerIffReader : IGameMakerIffDataHandler {
    public Encoding Encoding { get; }

    public byte[] Data { get; }

    public int Position { get; set; }

    public int Length => Data.Length;

    /// <summary>
    ///     A dictionary of pointers to objects that have been read.
    /// </summary>
    public Dictionary<int, IGameMakerSerializable> Pointers { get; set; } = new();

    /// <summary>
    ///     Initializes a new instance of <see cref="GameMakerIffReader"/>.
    /// </summary>
    /// <param name="data">The <see cref="Data"/> to read from.</param>
    /// <param name="encoding">The <see cref="Encoding"/> to read in.</param>
    public GameMakerIffReader(byte[] data, Encoding? encoding = null) {
        Data = data;
        Position = 0;
        Encoding = encoding ?? IGameMakerIffDataHandler.DEFAULT_ENCODING;
    }

    private unsafe T ReadGenericStruct<T>() where T : unmanaged {
        var val = Unsafe.As<byte, T>(ref Data[Position]);
        Position += sizeof(T);
        return val;
    }

    public Memory<byte> ReadBytes(int count) {
        Debug.Assert(Position >= 0 && Position + count <= Length);
        var bytes = Data.AsMemory(Position, count);
        Position += count;
        return bytes;
    }

    public char[] ReadChars(int count) {
        // TODO: Figure out best way to do this.
        // - Memory<byte>::ToArray()?
        // - Span<byte>::ToArray()?
        // The latter makes more sense.
        return Encoding.GetChars(ReadBytes(count).Span.ToArray());
    }

    public byte ReadByte() {
        Debug.Assert(Position >= 0 && Position < Length);
        return Data[Position++];
    }

    public bool ReadBoolean(bool wide) {
        return wide ? ReadInt32() != 0 : ReadByte() != 0;
    }

    public short ReadInt16() {
        Debug.Assert(Position >= 0 && Position + sizeof(short) <= Length);
        return ReadGenericStruct<short>();
    }

    public ushort ReadUInt16() {
        Debug.Assert(Position >= 0 && Position + sizeof(ushort) <= Length);
        return ReadGenericStruct<ushort>();
    }

    public unsafe int ReadInt24() {
        Debug.Assert(Position >= 0 && Position + sizeof(Int24) <= Length);
        return ReadGenericStruct<Int24>();
    }

    public unsafe uint ReadUInt24() {
        Debug.Assert(Position >= 0 && Position + sizeof(UInt24) <= Length);
        return ReadGenericStruct<UInt24>();
    }

    public int ReadInt32() {
        Debug.Assert(Position >= 0 && Position + sizeof(int) <= Length);
        return ReadGenericStruct<int>();
    }

    public uint ReadUInt32() {
        Debug.Assert(Position >= 0 && Position + sizeof(uint) <= Length);
        return ReadGenericStruct<uint>();
    }

    public long ReadInt64() {
        Debug.Assert(Position >= 0 && Position + sizeof(long) <= Length);
        return ReadGenericStruct<long>();
    }

    public ulong ReadUInt64() {
        Debug.Assert(Position >= 0 && Position + sizeof(ulong) <= Length);
        return ReadGenericStruct<ulong>();
    }

    public float ReadSingle() {
        Debug.Assert(Position >= 0 && Position + sizeof(float) <= Length);
        return ReadGenericStruct<float>();
    }

    public double ReadDouble() {
        Debug.Assert(Position >= 0 && Position + sizeof(double) <= Length);
        return ReadGenericStruct<double>();
    }

    public GameMakerPointer<T> ReadPointer<T>(int addr, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        if (useTypeOffset)
            addr -= GameMakerPointer.GetPointerOffset(typeof(T));

        var ptr = new GameMakerPointer<T> {
            Address = addr,
        };

        if (addr == 0)
            return ptr;

        if (Pointers.TryGetValue(addr, out var obj)) {
            ptr.Object = (T)obj;
            return ptr;
        }

        ptr.Object = new T();
        Pointers.Add(addr, ptr.Object);
        return ptr;
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="GameMakerIffReader"/> from
    ///     a <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <returns>An instance of <see cref="GameMakerIffReader"/>.</returns>
    /// <exception cref="IOException">
    ///     When the stream fails to read up to its length.
    /// </exception>
    public static GameMakerIffReader FromStream(Stream stream) {
        var data = new byte[stream.Length];
        var dataLength = stream.Read(data, 0, data.Length);
        if (dataLength != data.Length)
            throw new IOException($"Expected to read {data.Length} bytes, but only read {dataLength} bytes.");

        return new GameMakerIffReader(data);
    }
}

public static class GameMakerIffReaderExtensions {
    public delegate void ListRead(DeserializationContext context, int index, int count);

    public delegate GameMakerPointer<T> ListElementRead<T>(DeserializationContext context, bool notLast) where T : IGameMakerSerializable, new();

    public static Guid ReadGuid(this GameMakerIffReader reader) {
        return new Guid(reader.ReadBytes(16).Span);
    }

    public static GameMakerPointer<T> ReadPointerAndObject<T>(this GameMakerIffReader reader, int addr, DeserializationContext context, bool returnAfter, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        var ptr = reader.ReadPointer<T>(addr, useTypeOffset);
        ptr.ReadObject(context, returnAfter);
        return ptr;
    }

    public static List<GameMakerPointer<T>> ReadPointerList<T>(this GameMakerIffReader reader, DeserializationContext context, ListRead? beforeRead, ListRead? afterRead, ListElementRead<T>? elementReader)
        where T : IGameMakerSerializable, new() {
        elementReader ??= (ctx, _) => reader.ReadPointerAndObject<T>(reader.ReadInt32(), ctx, returnAfter: true);

        var count = reader.ReadInt32();
        var list = new List<GameMakerPointer<T>>(count);

        for (var i = 0; i < count; i++) {
            beforeRead?.Invoke(context, i, count);
            list.Add(elementReader(context, i != count - 1));
            afterRead?.Invoke(context, i, count);
        }

        return list;
    }
}
