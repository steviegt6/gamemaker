using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static class Shared {
    internal const StringMarshalling STRING_MARSHALLING =
#if x64
        StringMarshalling.Utf16;
#elif x86
        StringMarshalling.Utf8;
#elif AnyCPU
        StringMarshalling.Utf8; // We want to let AnyCPU compile.
#else
#error "Unsupported architecture"
#endif

    internal const UnmanagedType UNMANAGED_STRING_TYPE =
#if x64
        UnmanagedType.LPWStr;
#elif x86
        UnmanagedType.LPUTF8Str;
#elif AnyCPU
        UnmanagedType.LPUTF8Str; // We want to let AnyCPU compile.
#else
#error "Unsupported architecture"
#endif
}
