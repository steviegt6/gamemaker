using System;
using System.Runtime.CompilerServices;
using System.Text;
using Tomat.GameMaker.IFF.DataTypes;

namespace Tomat.GameMaker.IFF.IO;

public sealed class GameMakerIffWriter : IGameMakerIffDataHandler {
    public const int DEFAULT_CAPACITY = 1024 * 1024 * 10; // 10 MB

    public Encoding Encoding { get; }

    public byte[] Data => data;

    public int Position { get; set; }

    public int Length { get; set; }

    private byte[] data;

    /// <summary>
    ///     Initializes a new instance of <see cref="GameMakerIffWriter"/>.
    /// </summary>
    /// <param name="capacity">
    ///     The base size of the <see cref="Data"/> array.
    /// </param>
    /// <param name="encoding">The <see cref="Encoding"/> to write in.</param>
    public GameMakerIffWriter(int capacity = DEFAULT_CAPACITY, Encoding? encoding = null) {
        data = new byte[capacity];
        Position = 0;
        Length = 0;
        Encoding = encoding ?? IGameMakerIffDataHandler.DEFAULT_ENCODING;
    }

    private unsafe void WriteGenericStruct<T>(T value) where T : unmanaged {
        EnsureCapacity(Position + sizeof(T));
        Unsafe.As<byte, T>(ref Data[Position]) = value;
        // Pretty sure Unsafe.WriteUnaligned just calls Unsafe.As...
        // Unsafe.WriteUnaligned(ref Data[Position], Unsafe.As<T, byte>(ref value));
        Position += sizeof(T);
    }

    private void EnsureCapacity(int size) {
        if (Length >= size)
            return;

        var newSize = Math.Max(Length * 2, size);
        Array.Resize(ref data, newSize);
        Length = newSize;
    }

    public void Write(Memory<byte> value) {
        EnsureCapacity(Position + value.Length);
        // TODO: This *feels* dirty...
        value.CopyTo(Data.AsMemory().Slice(Position, value.Length));
        Position += value.Length;
    }

    public void Write(byte[] value) {
        EnsureCapacity(Position + value.Length);
        Buffer.BlockCopy(value, 0, Data, Position, value.Length);
        Position += value.Length;
    }

    public void Write(char[] value) {
        // This uses the Write(byte[]) overload.
        Write(Encoding.GetBytes(value));
    }

    public void Write(byte value) {
        EnsureCapacity(Position + sizeof(byte));
        Data[Position++] = value;
    }

    public void Write(bool value, bool wide) {
        if (wide)
            Write(value ? 1 : 0);
        else
            Write(value ? (byte)1 : (byte)0);
    }

    public void Write(short value) {
        EnsureCapacity(Position + sizeof(short));
        WriteGenericStruct(value);
    }

    public void Write(ushort value) {
        EnsureCapacity(Position + sizeof(ushort));
        WriteGenericStruct(value);
    }

    public unsafe void Write(Int24 value) {
        EnsureCapacity(Position + sizeof(Int24));
        WriteGenericStruct(value);
    }

    public unsafe void Write(UInt24 value) {
        EnsureCapacity(Position + sizeof(UInt24));
        WriteGenericStruct(value);
    }

    public void Write(int value) {
        EnsureCapacity(Position + sizeof(int));
        WriteGenericStruct(value);
    }

    public void Write(uint value) {
        EnsureCapacity(Position + sizeof(uint));
        WriteGenericStruct(value);
    }

    public void Write(long value) {
        EnsureCapacity(Position + sizeof(long));
        WriteGenericStruct(value);
    }

    public void Write(ulong value) {
        EnsureCapacity(Position + sizeof(ulong));
        WriteGenericStruct(value);
    }

    public void Write(float value) {
        EnsureCapacity(Position + sizeof(float));
        WriteGenericStruct(value);
    }

    public void Write(double value) {
        EnsureCapacity(Position + sizeof(double));
        WriteGenericStruct(value);
    }
}
