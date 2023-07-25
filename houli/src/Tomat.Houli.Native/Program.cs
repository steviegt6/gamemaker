using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tomat.Houli.Native;

internal static partial class Program {
    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new[] { typeof(CallConvStdcall) })]
    public static bool DllMain(nint hModule, uint ulReasonForCall, nint lpReserved) {
        Console.WriteLine("test");
        MessageBox(nint.Zero, "text", "caption", 0);
        return true;
    }

    [LibraryImport("user32.dll", EntryPoint = "MessageBoxW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    internal static partial int MessageBox(nint hWnd, string text, string caption, uint type);
}
