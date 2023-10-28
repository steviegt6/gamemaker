using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Tomat.GameMaker.IFF.Models;

namespace Tomat.GameMaker.IFF.IO;

public sealed class DeserializationContext {
    private static readonly Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    public HighCapacityArray<byte> Data { get; }

    public long Position { get; set; }

    public long Length => Data.Length;

    public DeserializationContext(HighCapacityArray<byte> data) {
        Data = data;
    }

#region Reading
    public byte ReadByte() {
        AssertAvailableSize<byte>();
        return Data[Position++];
    }

    public bool ReadBoolean(bool wide) {
        return wide ? ReadInt32() != 0 : ReadByte() != 0;
    }

    public short ReadInt16() {
        return ReadGenericStruct<short>();
    }

    public ushort ReadUInt16() {
        return ReadGenericStruct<ushort>();
    }

    public Int24 ReadInt24() {
        return ReadGenericStruct<Int24>();
    }

    public UInt24 ReadUInt24() {
        return ReadGenericStruct<UInt24>();
    }

    public int ReadInt32() {
        return ReadGenericStruct<int>();
    }

    public uint ReadUInt32() {
        return ReadGenericStruct<uint>();
    }

    public long ReadInt64() {
        return ReadGenericStruct<long>();
    }

    public ulong ReadUInt64() {
        return ReadGenericStruct<ulong>();
    }

    public float ReadSingle() {
        return ReadGenericStruct<float>();
    }

    public double ReadDouble() {
        return ReadGenericStruct<double>();
    }

    public Span<byte> ReadBytes(int count) {
        AssertAvailableSize(count);
        return Data.GetSpan(Position, count);
    }

    public char[] ReadChars(int count) {
        return encoding.GetChars(ReadBytes(count).ToArray());
    }

    private unsafe T ReadGenericStruct<T>() where T : unmanaged {
        AssertAvailableSize<T>();

        var size = sizeof(T);

        if (Data.IsContiguous(Position, Position + size)) {
            var result = Unsafe.ReadUnaligned<T>(ref Data.Ref(Position));
            Position += size;
            return result;
        }

        var buffer = stackalloc byte[size];
        for (var i = 0; i < size; i++)
            buffer[i] = Data[Position + i];

        Position += size;
        return Unsafe.ReadUnaligned<T>(buffer);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssertAvailableSize(int count) {
        Debug.Assert(Position >= 0 && Position + count <= Length);
    }

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void AssertAvailableSize<T>() where T : unmanaged {
        AssertAvailableSize(sizeof(T));
    }

    public static DeserializationContext FromPath(string path) {
        if (!Directory.Exists(path))
            throw new FileNotFoundException("Can not create deserialization context from directory, did you mean to pass a file path?", path);

        if (!File.Exists(path))
            throw new FileNotFoundException("Can not create deserialization context from file that does not exist.", path);

        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        return new DeserializationContext(HighCapacityArray.FromFileStream(fs));
    }
#endregion
}
