using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Tomat.GameMaker.IFF.Models;

namespace Tomat.GameMaker.IFF.IO;

public sealed class DeserializationContext {
    private static readonly Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    private readonly Stream stream;

    public long Position {
        get => stream.Position;
        set => stream.Position = value;
    }

    public long Length => stream.Length;

    public DeserializationContext(Stream stream) {
        this.stream = stream;
    }

#region Reading
    public byte ReadByte() {
        return ReadGenericStruct<byte>();
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

    public byte[] ReadBytes(int count) {
        AssertAvailableSize(count);
        var buffer = new byte[count];
        var read = stream.Read(buffer, 0, count);
        Debug.Assert(read == count);
        return buffer;
    }

    public char[] ReadChars(int count) {
        return encoding.GetChars(ReadBytes(count));
    }

    private unsafe T ReadGenericStruct<T>() where T : unmanaged {
        AssertAvailableSize<T>();

        var size = sizeof(T);
        Span<byte> buffer = stackalloc byte[size];
        var read = stream.Read(buffer);
        Debug.Assert(read == size);
        return Unsafe.ReadUnaligned<T>(ref buffer[0]);
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
        return new DeserializationContext(fs);
    }
#endregion
}
