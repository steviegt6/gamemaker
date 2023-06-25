using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Tomat.GameMaker.Benchmarks.Performance;

[MemoryDiagnoser]
public class FastReadBytes {
    [Params(1_000_000, 100_000_000)]
    public int Length;

    private int position;
    private byte[] u8 = null!;

    [IterationSetup]
    public void Setup() {
        u8 = new byte[Length];

        var rand = new Random();
        for (var i = 0; i < Length; i++)
            u8[i] = (byte)rand.Next(byte.MinValue, byte.MaxValue);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Memory<byte> ReadBytesMemoryAsMemory() {
        var bytes = u8.AsMemory(position, Length);
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Memory<byte> ReadBytesMemorySlice() {
        var bytes = u8[position..(position + Length)];
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Memory<byte> ReadBytesMemoryConstructor() {
        var bytes = new Memory<byte>(u8, position, Length);
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public byte[] ReadBytesArray() {
        var bytes = new byte[Length];
        for (var i = 0; i < Length; i++)
            bytes[i] = u8[position++];

        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public byte[] ReadBytesArraySlice() {
        var bytes = u8[position..(position + Length)];
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public byte[] ReadBytesArrayCopy() {
        var bytes = new byte[Length];
        Array.Copy(u8, position, bytes, 0, Length);
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public byte[] ReadBytesBufferBlockCopy() {
        var bytes = new byte[Length];
        Buffer.BlockCopy(u8, position, bytes, 0, Length);
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public unsafe byte[] ReadBytesBufferMemoryCopy() {
        var bytes = new byte[Length];

        fixed (byte* source = &u8[0])
        fixed (byte* dest = &bytes[0])
            Buffer.MemoryCopy(source + position, dest, Length, Length);

        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Span<byte> ReadBytesSpanConstructor() {
        var bytes = new Span<byte>(u8, position, Length);
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Span<byte> ReadBytesMemoryMarshalCreateSpan() {
        var bytes = MemoryMarshal.CreateSpan(ref u8[position], Length);
        position += Length;
        return bytes;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Memory<byte> ReadBytesMemoryMarshalCreateFromPinnedArray() {
        var bytes = MemoryMarshal.CreateFromPinnedArray(u8, position, Length);
        position += Length;
        return bytes;
    }

    [Benchmark]
    public void ReadBytesMemoryAsMemoryRead() {
        _ = ReadBytesMemoryAsMemory();
        position = 0;
    }

    // Slooooooooooooooow...
    /*[Benchmark]
    public void ReadBytesMemorySliceRead() {
        _ = ReadBytesMemorySlice();
        position = 0;
    }*/

    [Benchmark]
    public void ReadBytesMemoryConstructorRead() {
        _ = ReadBytesMemoryConstructor();
        position = 0;
    }

    // Slooooooooooooooow...
    /*[Benchmark]
    public void ReadBytesArrayRead() {
        _ = ReadBytesArray();
        position = 0;
    }*/

    // Slooooooooooooooow...
    /*[Benchmark]
    public void ReadBytesArraySliceRead() {
        _ = ReadBytesArraySlice();
        position = 0;
    }

    [Benchmark]
    public void ReadBytesArrayCopyRead() {
        _ = ReadBytesArrayCopy();
        position = 0;
    }

    [Benchmark]
    public void ReadBytesBufferBlockCopyRead() {
        _ = ReadBytesBufferBlockCopy();
        position = 0;
    }

    [Benchmark]
    public void ReadBytesBufferMemoryCopyRead() {
        _ = ReadBytesBufferMemoryCopy();
        position = 0;
    }*/

    [Benchmark]
    public void ReadBytesSpanConstructorRead() {
        _ = ReadBytesSpanConstructor();
        position = 0;
    }

    [Benchmark]
    public void ReadBytesMemoryMarshalCreateSpanRead() {
        _ = ReadBytesMemoryMarshalCreateSpan();
        position = 0;
    }

    [Benchmark]
    public void ReadBytesMemoryMarshalCreateFromPinnedArrayRead() {
        _ = ReadBytesMemoryMarshalCreateFromPinnedArray();
        position = 0;
    }
}
