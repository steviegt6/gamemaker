using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;
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
    ///     A map of pointers to the addresses in which their objects should be
    ///     written.
    /// </summary>
    public Dictionary<IGameMakerSerializable, int> Pointers { get; set; } = new();

    /// <summary>
    ///     A map of pointers to the addresses in which they are referenced.
    /// </summary>
    public Dictionary<IGameMakerSerializable, List<(int, bool)>> PointerReferences { get; set; } = new();

    // public List<IGameMakerPointer> 

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
        if (Data.Length >= size)
            return;

        var newSize = Math.Max(Data.Length * 2, size);
        Array.Resize(ref data, newSize);
        Length = size;
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

    public unsafe void Write(Int24 value) {
        WriteGenericStruct(value);
    }

    public unsafe void Write(UInt24 value) {
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
        // Checking only for null and not just default; pointers should only be
        // getting used for reference types anyway...
        if (ptr.Object is null) {
            Write(0);
            return;
        }

        if (PointerReferences.TryGetValue(ptr.Object, out var references)) {
            references.Add((Position, useTypeOffset));
        }
        else {
            PointerReferences[ptr.Object] = new List<(int, bool)> {
                (Position, useTypeOffset),
            };
        }

        Write(0); // Placeholder for the address.
    }

    public void FinalizePointers() {
        foreach (var kvp in PointerReferences) {
            if (Pointers.TryGetValue(kvp.Key, out var ptr)) {
                foreach (var addr in kvp.Value)
                    this.WriteAt(addr.Item1, ptr + (addr.Item2 ? GameMakerPointer.GetPointerOffset(kvp.Key.GetType()) : 0));
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
                    ptr += GameMakerPointer.GetPointerOffset(kvp.Key.GetType());

                    foreach (var addr in kvp.Value)
                        this.WriteAt(addr, ptr);
                }
                else {
                    foreach (var addr in kvp.Value)
                        this.WriteAt(addr, 0);
                }
            }
        );*/
    }
}

public static class GameMakerIffWriterExtensions {
    public delegate void ListWrite(SerializationContext context, int index, int count);

    public delegate void ListElementWrite<T>(SerializationContext context, GameMakerPointer<T> element) where T : IGameMakerSerializable, new();

    public static void Pad(this GameMakerIffWriter writer, int align) {
        var pad = writer.Position % align;
        if (pad == 0)
            return;

        writer.Write(new byte[align - pad]);
    }

    public static void WriteAt(this GameMakerIffWriter writer, int position, int value) {
        var oldPos = writer.Position;
        writer.Position = position;
        writer.Write(value);
        writer.Position = oldPos;
    }

    public static int BeginLength(this GameMakerIffWriter writer) {
        writer.Write(0);
        return writer.Position;
    }

    public static void EndLength(this GameMakerIffWriter writer, int beginPos) {
        var pos = writer.Position;
        writer.Position = beginPos - 4;
        writer.Write(pos - beginPos);
        writer.Position = pos;
    }

    public static void WritePointerList<T>(this GameMakerIffWriter writer,
                                           List<GameMakerPointer<T>> list,
                                           SerializationContext context,
                                           ListWrite? beforeWriter,
                                           ListWrite? afterWriter,
                                           ListElementWrite<T>? elementWriter,
                                           ListElementWrite<T>? elementPointerWriter) where T : IGameMakerSerializable, new() {
        writer.Write(list.Count);

        // Write pointers.
        foreach (var item in list) {
            if (elementPointerWriter is null)
                writer.Write(item);
            else
                elementPointerWriter.Invoke(context, item);
        }

        // Write elements.
        for (var i = 0; i < list.Count; i++) {
            beforeWriter?.Invoke(context, i, list.Count);

            if (elementWriter is null) {
                list[i].WriteObject(context);
                list[i].Object!.Write(context);
            }
            else {
                elementWriter.Invoke(context, list[i]);
            }

            afterWriter?.Invoke(context, i, list.Count);
        }
    }
}
