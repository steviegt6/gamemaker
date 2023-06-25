using System;
using BenchmarkDotNet.Attributes;

namespace Tomat.GameMaker.Benchmarks.Performance;

[MemoryDiagnoser]
public class FastReadByte {
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

    private byte ReadByteFromArray() {
        return u8[position++];
    }

    private unsafe byte ReadByteFromPointer() {
        fixed (byte* ptr = &u8[position]) {
            position++;
            return *ptr;
        }
    }

    [Benchmark]
    public void ArrayRead() {
        var bytes = new byte[Length];
        for (var i = 0; i < Length; i++)
            bytes[i] = ReadByteFromArray();

        position = 0;
    }

    [Benchmark]
    public void PointerRead() {
        var bytes = new byte[Length];
        for (var i = 0; i < Length; i++)
            bytes[i] = ReadByteFromPointer();

        position = 0;
    }
}
