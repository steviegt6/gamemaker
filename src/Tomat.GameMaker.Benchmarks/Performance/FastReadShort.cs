using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Tomat.GameMaker.Benchmarks.Performance;

[MemoryDiagnoser]
public class FastReadShort {
    [Params(1_000_000, 100_000_000)]
    public int Length;

    private int position;
    private byte[] u8 = null!;

    [IterationSetup]
    public void Setup() {
        u8 = new byte[Length * sizeof(short)];

        var rand = new Random();
        for (var i = 0; i < Length; i++)
            u8[i] = (byte)rand.Next(byte.MinValue, byte.MaxValue);
    }

    private short ReadShortFromBitConverter() {
        var val = BitConverter.ToInt16(u8, position);
        position += sizeof(short);
        return val;
    }

    private short ReadShortFromArray() {
        return (short)(u8[position++] | (u8[position++] << 8));
    }

    private unsafe short ReadShortFromPointer() {
        fixed (byte* ptr = &u8[position]) {
            position += sizeof(short);
            return *(short*)ptr;
        }
    }

    private short ReadShortFromUnsafeAs() {
        var val = Unsafe.As<byte, short>(ref u8[position]);
        position += sizeof(short);
        return val;
    }

    private short ReadShortFromUnsafeReadUnaligned() {
        var val = Unsafe.ReadUnaligned<short>(ref u8[position]);
        position += sizeof(short);
        return val;
    }

    private unsafe short ReadShortFromUnsafeRead() {
        fixed (byte* ptr = &u8[position]) {
            var val = Unsafe.Read<short>(ptr);
            position += sizeof(short);
            return val;
        }
    }

    private unsafe T ReadGenericFromPointer<T>() where T : unmanaged {
        fixed (byte* ptr = &u8[position]) {
            position += sizeof(T);
            return *(T*)ptr;
        }
    }

    private unsafe T ReadGenericFromUnsafeAs<T>() where T : unmanaged {
        var val = Unsafe.As<byte, T>(ref u8[position]);
        position += sizeof(T);
        return val;
    }

    private unsafe T ReadGenericFromUnsafeReadUnaligned<T>() where T : unmanaged {
        var val = Unsafe.ReadUnaligned<T>(ref u8[position]);
        position += sizeof(T);
        return val;
    }

    private unsafe T ReadGenericFromUnsafeRead<T>() where T : unmanaged {
        fixed (byte* ptr = &u8[position]) {
            var val = Unsafe.Read<T>(ptr);
            position += sizeof(T);
            return val;
        }
    }

    [Benchmark]
    public void BitConverterRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadShortFromBitConverter();

        position = 0;
    }

    [Benchmark]
    public void ArrayRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadShortFromArray();

        position = 0;
    }

    [Benchmark]
    public void PointerRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadShortFromPointer();

        position = 0;
    }

    [Benchmark]
    public void UnsafeAsRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadShortFromUnsafeAs();

        position = 0;
    }

    [Benchmark]
    public void UnsafeReadUnalignedRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadShortFromUnsafeReadUnaligned();

        position = 0;
    }

    [Benchmark]
    public void UnsafeReadURead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadShortFromUnsafeRead();

        position = 0;
    }

    [Benchmark]
    public void GenericPointerRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadGenericFromPointer<short>();

        position = 0;
    }

    [Benchmark]
    public void GenericUnsafeAsRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadGenericFromUnsafeAs<short>();

        position = 0;
    }

    [Benchmark]
    public void GenericUnsafeReadUnalignedRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadGenericFromUnsafeReadUnaligned<short>();

        position = 0;
    }

    [Benchmark]
    public void GenericUnsafeReadRead() {
        var shorts = new short[Length];
        for (var i = 0; i < Length; i++)
            shorts[i] = ReadGenericFromUnsafeRead<short>();

        position = 0;
    }
}
