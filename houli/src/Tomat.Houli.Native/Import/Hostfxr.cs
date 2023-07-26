using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static class Hostfxr {
    public enum HostfxrDelegateType {
        HdtComActivation,
        HdtLoadInMemoryAssembly,
        HdtWinrtActivation,
        HdtComRegister,
        HdtComUnregister,
        HdtLoadAssemblyAndGetFunctionPointer,
        HdtGetFunctionPointer,
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct HostfxrInitializeParameters {
        public readonly nint Size;
        public readonly nint HostPath;
        public readonly nint DotnetRoot;

        public HostfxrInitializeParameters(nint size, nint hostPath, nint dotnetRoot) {
            Size = size;
            HostPath = hostPath;
            DotnetRoot = dotnetRoot;
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint HostfxrMainFn(
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = Shared.UNMANAGED_STRING_TYPE)]
        string[] argv
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint HostfxrMainStartupInfoFn(
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = Shared.UNMANAGED_STRING_TYPE)]
        string[] argv,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string hostPath,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string dotnetRoot,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string appPath
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int HostfxrMainBundleStartupinfoFn(
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = Shared.UNMANAGED_STRING_TYPE)]
        string[] argv,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string hostPath,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string dotnetRoot,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string appPath,
        long bundleHeaderOffset
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void HostfxrErrorWriterFn(
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string message
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate HostfxrErrorWriterFn SetErrorWriterFn(
        HostfxrErrorWriterFn errorWriter
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int HostfxrInitializeForDotnetCommandLineFn(
        int argc,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = Shared.UNMANAGED_STRING_TYPE)]
        string[] argv,
        in HostfxrInitializeParameters parameters,
        out nint hostfxrHandle
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int HostfxrInitializeForRuntimeConfigFn(
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string runtimeConfigPath,
        /*in HostfxrInitializeParameters*/ HostfxrInitializeParameters* parameters,
        out nint hostfxrHandle
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int HostfxrGetRuntimePropertyFn(
        nint hostfxrHandle,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string name,
        out nint value
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int HostfxrSetRuntimePropertyFn(
        nint hostfxrHandle,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string name,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string value
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int HostfxrRunAppFn(
        nint hostfxrHandle
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int HostfxrGetRuntimeDelegateFn(
        nint hostfxrHandle,
        HostfxrDelegateType type,
        out nint functionPointer
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int HostfxrCloseFn(
        nint hostfxrHandle
    );
}
