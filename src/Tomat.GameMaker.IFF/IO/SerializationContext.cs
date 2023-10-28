using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tomat.GameMaker.IFF.Models;

namespace Tomat.GameMaker.IFF.IO;

public sealed class SerializationContext : IDisposable,
                                           IAsyncDisposable {
    private static readonly Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    private readonly Stream stream;

    public long Position {
        get => stream.Position;
        set => stream.Position = value;
    }

    public long Length => stream.Length;

    public SerializationContext(Stream stream) {
        this.stream = stream;
    }

#region Writing
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

    public void Write(byte[] value) {
        stream.Write(value);
    }

    public void Write(Span<byte> value) {
        stream.Write(value);
    }

    public void Write(char[] value) {
        Write(encoding.GetBytes(value));
    }

    private unsafe void WriteGenericStruct<T>(T value) where T : unmanaged {
        Span<byte> buffer = stackalloc byte[sizeof(T)];
        Unsafe.WriteUnaligned(ref buffer[0], value);
        stream.Write(buffer);
    }
#endregion

#region IDisposable implementation
    public void Dispose() {
        stream.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await stream.DisposeAsync();
    }
#endregion
}
