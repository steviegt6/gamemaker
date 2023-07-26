using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static class CoreclrDelegates {
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int LoadAssemblyAndGetFunctionPointerFn(
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string assemblyPath,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string typeName,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string methodName,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        nint delegateTypeName,
        nint reserved,
        nint* delegateFnPtr
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int ComponentEntryPointFn(
        nint arg,
        int argSizeInBytes
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetFunctionPointerFn(
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string typeName,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string methodName,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        string delegateTypeName,
        [MarshalAs(Shared.UNMANAGED_STRING_TYPE)]
        nint loadContext,
        nint reserved,
        out nint @delegate
    );
}
