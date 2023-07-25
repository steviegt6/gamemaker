using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static class CoreclrDelegates {
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

    public unsafe delegate int LoadAssemblyAndGetFunctionPointerFn(
        [MarshalAs(unmanaged_string_type)]
        string assemblyPath,
        [MarshalAs(unmanaged_string_type)]
        string typeName,
        [MarshalAs(unmanaged_string_type)]
        string methodName,
        [MarshalAs(unmanaged_string_type)]
        string delegateTypeName,
        void* reserved,
        out void* delegateFnPtr
    );

    public unsafe delegate int ComponentEntryPointFn(
        void* arg,
        int argSizeInBytes
    );

    // get function pointer fn
    public unsafe delegate int GetFunctionPointerFn(
        [MarshalAs(unmanaged_string_type)]
        string typeName,
        [MarshalAs(unmanaged_string_type)]
        string methodName,
        [MarshalAs(unmanaged_string_type)]
        string delegateTypeName,
        [MarshalAs(unmanaged_string_type)]
        void* loadContext,
        void* reserved,
        void** @delegate
    );
}
