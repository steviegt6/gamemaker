using System;
using System.IO;
using System.Linq;

namespace Tomat.GameMaker.IFF.IO;

public sealed class HighCapacityArray<T> where T : unmanaged {
    private readonly T[][] arrays;

    public long Length => arrays.Sum(x => x.Length);

    public T this[long index] {
        get => arrays[index / int.MaxValue][index % int.MaxValue];
        set => arrays[index / int.MaxValue][index % int.MaxValue] = value;
    }

    public HighCapacityArray(T[][] arrays) {
        this.arrays = arrays;
    }

    public bool IsContiguous(long index1, long index2) {
        return index1 / int.MaxValue == index2 / int.MaxValue;
    }

    public ref T Ref(long index) {
        return ref arrays[index / int.MaxValue][index % int.MaxValue];
    }

    public unsafe Span<T> GetSpan(long index, long length) {
        if (IsContiguous(index, index + length))
            return arrays[index / int.MaxValue].AsSpan((int) (index % int.MaxValue), (int) length);

        var span = new T[length];

        fixed (T* ptr = span) {
            var ptr2 = ptr;

            for (var i = 0; i < length; i++) {
                *ptr2 = arrays[(index + i) / int.MaxValue][(index + i) % int.MaxValue];
                ptr2++;
            }
        }

        return span;
    }
}

public static class HighCapacityArray {
    public static HighCapacityArray<byte> FromFileStream(FileStream fs) {
        var arrayCount = fs.Length / int.MaxValue + 1;
        var arrays = new byte[arrayCount][];

        for (var i = 0; i < arrayCount; i++) {
            var arrayLength = i == arrayCount - 1 ? fs.Length % int.MaxValue : int.MaxValue;
            arrays[i] = new byte[arrayLength];
            var read = fs.Read(arrays[i], 0, (int) arrayLength);
            if (read != arrayLength)
                throw new IOException($"Expected to read {arrayLength} bytes, but only read {read}.");
        }

        return new HighCapacityArray<byte>(arrays);
    }
}
