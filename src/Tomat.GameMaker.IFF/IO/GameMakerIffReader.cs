using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;
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

    public T? ReadPointer<T>(int ptr) where T : IGameMakerSerializable, new() {
        if (ptr == 0)
            return default;

        if (Pointers.TryGetValue(ptr, out var obj))
            return (T)obj;

        obj = new T();
        Pointers.Add(ptr, obj);
        return (T)obj;
    }

    public T? ReadPointerObject<T>(int ptr, DeserializationContext context, bool returnAfter) where T : IGameMakerSerializable, new() {
        if (ptr == 0)
            return default;

        T res;

        if (Pointers.TryGetValue(ptr, out var obj)) {
            res = (T)obj;
        }
        else {
            res = new T();
            Pointers[ptr] = res;
        }

        var pos = Position;
        Position = ptr;

        res.Read(context);

        if (returnAfter)
            Position = pos;

        return res;
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
    public static T? ReadPointer<T>(this GameMakerIffReader reader) where T : IGameMakerSerializable, new() {
        return reader.ReadPointer<T>(reader.ReadInt32());
    }

    public static T? ReadPointerObject<T>(this GameMakerIffReader reader, DeserializationContext context, bool returnAfter) where T : IGameMakerSerializable, new() {
        return reader.ReadPointerObject<T>(reader.ReadInt32(), context, returnAfter);
    }

    public static Guid ReadGuid(this GameMakerIffReader reader) {
        return new Guid(reader.ReadBytes(16).Span);
    }

    public static GameMakerString? ReadStringPointer(this GameMakerIffReader reader) {
        return reader.ReadPointer<GameMakerString>(reader.ReadInt32() - 4);
    }

    public static GameMakerString? ReadStringPointerObject(this GameMakerIffReader reader, DeserializationContext context, bool returnAfter) {
        return reader.ReadPointerObject<GameMakerString>(reader.ReadInt32() - 4, context, returnAfter);
    }
}
