using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static class Hostfxr {
    public struct HostfxrInitializeParameters {
        public nint Size;
        public nint HostPath;
        public nint DotnetRoot;

        public HostfxrInitializeParameters(nint size, nint hostPath, nint dotnetRoot) {
            Size = size;
            HostPath = hostPath;
            DotnetRoot = dotnetRoot;
        }
    }

    public enum HostfxrDelegateType {
        HdtComActivation,
        HdtLoadInMemoryAssembly,
        HdtWinrtActivation,
        HdtComRegister,
        HdtComUnregister,
        HdtLoadAssemblyAndGetFunctionPointer,
        HdtGetFunctionPointer,
    };

    private const StringMarshalling string_marshalling =
#if x64
        StringMarshalling.Utf16;
#elif x86
        StringMarshalling.Utf8;
#elif AnyCPU
        StringMarshalling.Utf8; // We want to let AnyCPU compile.
#else
#error "Unsupported architecture"
#endif

    private const UnmanagedType unmanaged_string_type =
#if x64
        UnmanagedType.LPWStr;
#elif x86
        UnmanagedType.LPUTF8Str;
#elif AnyCPU
        UnmanagedType.LPUTF8Str; // We want to let AnyCPU compile.
#else
#error "Unsupported architecture"
#endif

    public delegate nint HostfxrMainFn(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = unmanaged_string_type)] string[] argv);

    public delegate nint HostfxrMainStartupInfoFn(
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = unmanaged_string_type)]
        string[] argv,
        [MarshalAs(unmanaged_string_type)]
        string hostPath,
        [MarshalAs(unmanaged_string_type)]
        string dotnetRoot,
        [MarshalAs(unmanaged_string_type)]
        string appPath
    );

    public delegate int HostfxrMainBundleStartupinfoFn(
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = unmanaged_string_type)]
        string[] argv,
        [MarshalAs(unmanaged_string_type)]
        string hostPath,
        [MarshalAs(unmanaged_string_type)]
        string dotnetRoot,
        [MarshalAs(unmanaged_string_type)]
        string appPath,
        long bundleHeaderOffset
    );

    public delegate void HostfxrErrorWriterFn([MarshalAs(unmanaged_string_type)] string message);

    // TODO: idk if this is valid but I never use it lol
    public delegate HostfxrErrorWriterFn SetErrorWriterFn(HostfxrErrorWriterFn errorWriter);

    public unsafe delegate int HostfxrInitializeForDotnetCommandLineFn(
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = unmanaged_string_type)]
        string[] argv,
        HostfxrInitializeParameters* parameters,
        out nint hostfxrHandle
    );

    public unsafe delegate int HostfxrInitializeForRuntimeConfigFn(
        [MarshalAs(unmanaged_string_type)]
        string runtimeConfigPath,
        HostfxrInitializeParameters* parameters,
        out nint hostfxrHandle
    );

    public delegate int HostfxrGetRuntimePropertyFn(
        nint hostfxrHandle,
        [MarshalAs(unmanaged_string_type)]
        string name,
        out nint value
    );

    public delegate int HostfxrSetRuntimePropertyFn(
        nint hostfxrHandle,
        [MarshalAs(unmanaged_string_type)]
        string name,
        [MarshalAs(unmanaged_string_type)]
        string value
    );

    public delegate int HostfxrRunAppFn(
        nint hostfxrHandle
    );

    public delegate int HostfxrGetRuntimeDelegateFn(
        nint hostfxrHandle,
        HostfxrDelegateType type,
        out nint functionPointer
    );

    public delegate int HostfxrCloseFn(
        nint hostfxrHandle
    );
}
