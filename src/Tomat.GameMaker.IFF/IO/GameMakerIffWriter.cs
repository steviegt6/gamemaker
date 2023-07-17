using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes;

namespace Tomat.GameMaker.IFF.IO;

public sealed class GameMakerIffWriter : IGameMakerIffWriter {
    public const int DEFAULT_CAPACITY = 1024 * 1024 * 10; // 10 MB

    public Encoding Encoding { get; }

    public byte[] Data => data;

    public int Position { get; set; }

    public int Length { get; set; }

    private byte[] data;

    public Dictionary<IGameMakerSerializable, int> Pointers { get; set; } = new();

    public Dictionary<IGameMakerSerializable, List<(int, bool)>> PointerReferences { get; set; } = new();

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
        if (Data.Length < size) {
            var newSize = Math.Max(Data.Length * 2, size);
            Array.Resize(ref data, newSize);
        }

        Length = Math.Max(Length, size);
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
        WriteGenericStruct(value);
    }

    public void Write(ushort value) {
        WriteGenericStruct(value);
    }

    public void Write(Int24 value) {
        WriteGenericStruct(value);
    }

    public void Write(UInt24 value) {
        WriteGenericStruct(value);
    }

    public void Write(int value) {
        WriteGenericStruct(value);
    }

    public void Write(uint value) {
        WriteGenericStruct(value);
    }

    public void Write(long value) {
        WriteGenericStruct(value);
    }

    public void Write(ulong value) {
        WriteGenericStruct(value);
    }

    public void Write(float value) {
        WriteGenericStruct(value);
    }

    public void Write(double value) {
        WriteGenericStruct(value);
    }

    public void Write<T>(GameMakerPointer<T> ptr, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        if (ptr.IsNull) {
            Write(0);
            return;
        }

        if (!ptr.TryGetObject(out var obj))
            throw new InvalidOperationException("Pointer object was null despite pointer not being null!");

        if (PointerReferences.TryGetValue(obj, out var references)) {
            references.Add((Position, useTypeOffset));
        }
        else {
            PointerReferences[obj] = new List<(int, bool)> {
                (Position, useTypeOffset),
            };
        }

        Write(0); // Placeholder for the address.
    }

    public void FinalizePointers() {
        foreach (var kvp in PointerReferences) {
            if (Pointers.TryGetValue(kvp.Key, out var ptr)) {
                foreach (var addr in kvp.Value)
                    this.WriteAt(addr.Item1, ptr + (addr.Item2 ? GameMakerPointerExtensions.GetPointerOffset(kvp.Key.GetType()) : 0));
            }
            else {
                foreach (var addr in kvp.Value)
                    this.WriteAt(addr.Item1, 0);
            }
        }
        /*Parallel.ForEach(
            PointerReferences,
            kvp => {
                if (Pointers.TryGetValue(kvp.Key, out var ptr)) {
                    foreach (var addr in kvp.Value)
                        this.WriteAt(addr.Item1, ptr + (addr.Item2 ? GameMakerPointerExtensions.GetPointerOffset(kvp.Key.GetType()) : 0));
                }
                else {
                    foreach (var addr in kvp.Value)
                        this.WriteAt(addr.Item1, 0);
                }
            }
        );*/
    }
}
