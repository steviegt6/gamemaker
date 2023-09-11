using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes;

namespace Tomat.GameMaker.IFF.IO;

/// <summary>
///     Responsible for reading data from a GameMaker IFF file.
/// </summary>
public sealed class GameMakerIffReader : IGameMakerIffReader {
    public Encoding Encoding { get; }

    public byte[] Data { get; }

    public int Position { get; set; }

    public int Length => Data.Length;

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

    public unsafe Int24 ReadInt24() {
        Debug.Assert(Position >= 0 && Position + sizeof(Int24) <= Length);
        return ReadGenericStruct<Int24>();
    }

    public unsafe UInt24 ReadUInt24() {
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

    /// <summary>
    ///     Creates a <see cref="GameMakerPointer{T}"/> that points to the
    ///     specified <paramref name="addr"/>, which is optionally offset by
    ///     <paramref name="useTypeOffset"/>. Once a pointer is read, it is
    ///     added to the <see cref="Pointers"/> map, and any future reads of
    ///     the same pointer address (regardless of location) will return the
    ///     same object.
    /// </summary>
    /// <param name="addr">The address of the pointer.</param>
    /// <param name="useTypeOffset">
    ///     Whether to use the type offset of <typeparamref name="TInterface" />.
    /// </param>
    /// <typeparam name="TInterface">The pointer type.</typeparam>
    /// <typeparam name="TImplementation">The implementing type.</typeparam>
    /// <returns>
    ///     A pointer pointing to the given <paramref name="addr"/> which
    ///     accesses the <see cref="Pointers"/> map.
    /// </returns>
    public GameMakerPointer<TInterface> ReadPointer<TInterface, TImplementation>(
        int addr,
        bool useTypeOffset = true
    ) where TInterface : IGameMakerSerializable
      where TImplementation : TInterface, new() {
        // FIX: If address is zero (null), just return a default (null) pointer.
        // This placement is only particularly important for GameMaker objects
        // with expected pointer offsets (e.g. GameMakerString) since a zeroed
        // address (null) is not offset like normal.
        if (addr == 0)
            return new GameMakerPointer<TInterface>();

        if (useTypeOffset)
            addr -= GameMakerPointerExtensions.GetPointerOffset(typeof(TInterface));

        var ptr = new GameMakerPointer<TInterface> {
            Address = addr,
        };

        ptr.GetOrInitializePointerObject<TImplementation>(this);
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
