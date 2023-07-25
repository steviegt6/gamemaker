/*using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Tomat.Houli.Native.Util;

/// <summary>
///     A string marshaller which marshals strings as ANSI strings in 32-bit
///     processes and Unicode strings in 64-bit processes.
/// </summary>
[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(AnsiUniArchitectureDependentStringMarshaller))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
public static unsafe class AnsiUniArchitectureDependentStringMarshaller {
    public ref struct ManagedToUnmanagedIn {
        // Optimized marshalling size...?
        public static int BufferSize => 0x100;

        private byte* unmanagedValue;
        private bool allocated;

        public void FromManaged(string? managed, Span<byte> buffer) {
            allocated = false;

            if (managed is null) {
                unmanagedValue = null;
                return;
            }

            // >= for null terminator.
            // Cast to long to avoid the checked operation.
            if ((long)Marshal.SystemMaxDBCSCharSize * managed.Length >= buffer.Length) {
                var exactByteCount = Environment.Is64BitProcess ? GetUniStringByteCount(managed) : GetAnsiStringByteCount(managed);

                if (exactByteCount > buffer.Length) {
                    buffer = new Span<byte>((byte*)NativeMemory.Alloc((nuint)exactByteCount), exactByteCount);
                    allocated = true;
                }
            }

            unmanagedValue = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(buffer));

            if (Environment.Is64BitProcess)
                GetUniStringBytes(managed, buffer);
            else
                GetAnsiStringBytes(managed, buffer);
        }

        public byte* ToUnmanaged() {
            return unmanagedValue;
        }

        public void Free() {
            if (allocated)
                NativeMemory.Free(unmanagedValue);
        }
    }
    
    // Not required for the marshaller, this is a utility.
    public static int CharSize => Environment.Is64BitProcess ? 2 : 1;

    public static byte* ConvertToUnmanaged(string? managed) {
        if (managed is null)
            return null;

        var exactByteCount = Environment.Is64BitProcess ? GetUniStringByteCount(managed.AsSpan()) : GetAnsiStringByteCount(managed.AsSpan());
        var mem = (byte*)Marshal.AllocCoTaskMem(exactByteCount);
        var buffer = new Span<byte>(mem, exactByteCount);

        if (Environment.Is64BitProcess)
            GetUniStringBytes(managed, buffer);
        else
            GetAnsiStringBytes(managed, buffer);

        return mem;
    }

    public static string? ConvertToManaged(byte* unmanaged) {
        return Environment.Is64BitProcess ? Marshal.PtrToStringUni((nint)unmanaged) : Marshal.PtrToStringAnsi((nint)unmanaged);
    }

    public static void Free(byte* unmanaged) {
        Marshal.FreeCoTaskMem((nint)unmanaged);
    }

    private static int GetAnsiStringByteCount(ReadOnlySpan<char> chars) {
        var byteLength = Encoding.UTF8.GetByteCount(chars);
        return unchecked(byteLength + 1);
    }

    private static int GetUniStringByteCount(ReadOnlySpan<char> chars) {
        var byteLength = Encoding.Unicode.GetByteCount(chars);
        return unchecked(byteLength + 2);
    }

    private static void GetAnsiStringBytes(ReadOnlySpan<char> chars, Span<byte> bytes) {
        var actualByteLength = Encoding.UTF8.GetBytes(chars, bytes);
        bytes[actualByteLength] = 0;
    }

    private static void GetUniStringBytes(ReadOnlySpan<char> chars, Span<byte> bytes) {
        var actualByteLength = Encoding.Unicode.GetBytes(chars, bytes);
        bytes[actualByteLength] = 0;
        bytes[actualByteLength + 1] = 0;
    }
}*/


