using System;
using System.Runtime.InteropServices;

namespace Tomat.Houli.API;

internal static class NativeEntry {
    [UnmanagedCallersOnly]
    internal static void Initialize(nint pBaseDirectory) {
        // get string from ansi char ptr
        var baseDirectory = Environment.Is64BitProcess ? Marshal.PtrToStringUni(pBaseDirectory) : Marshal.PtrToStringAnsi(pBaseDirectory);
        AppContext.SetData("APP_CONTEXT_BASE_DIRECTORY", baseDirectory);

        Console.WriteLine($"Hi, I'm .NET! {baseDirectory}");
    }
}
