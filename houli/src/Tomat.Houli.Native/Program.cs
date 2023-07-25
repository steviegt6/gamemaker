using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tomat.Houli.Native;

internal static partial class Program {
    public const int DLL_PROCESS_ATTACH = 1;
    public const int DLL_THREAD_ATTACH = 2;

    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new[] { typeof(CallConvStdcall) })]
    public static bool DllMain(nint hModule, uint ulReasonForCall, nint lpReserved) {
        switch (ulReasonForCall) {
            case DLL_PROCESS_ATTACH:
            case DLL_THREAD_ATTACH:
                MessageBox(nint.Zero, "text", "caption", 0);
                break;
        }

        return true;
    }

    [LibraryImport("user32.dll", EntryPoint = "MessageBoxW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    internal static partial int MessageBox(nint hWnd, string text, string caption, uint type);
}
