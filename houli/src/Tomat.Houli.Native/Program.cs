using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tomat.Houli.Native.Import;
using Tomat.Houli.Native.Util;

namespace Tomat.Houli.Native;

internal static class Program {
    private const int max_path = 256;
    public const int DLL_PROCESS_ATTACH = 1;

    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new[] { typeof(CallConvStdcall) })]
    public static bool DllMain(nint hModule, uint ulReasonForCall, nint lpReserved) {
        if (ulReasonForCall == DLL_PROCESS_ATTACH) {
            // If anything fails, it's reported in the method. We can return
            // from there.
            if (!ConsoleWindow.Initialize())
                return false;

            if (!LoadHostfxr()) {
                User32.MessageBox(nint.Zero, "Failed to lost hostfxr, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }
        }

        return true;
    }

    private static unsafe bool LoadHostfxr() {
        var buffer = new char[max_path];

        // No need to divide by char size unlike in native... probably?
        var bufferSize = (nint)buffer.Length;

        int rc;

        fixed (char* bufferPtr = buffer) {
            rc = NetHost.GetHostfxrPath(bufferPtr, &bufferSize, null);
        }

        if (rc != 0) {
            User32.MessageBox(nint.Zero, $"Failed to get hostfxr path with error code: {rc:x8}", "Error", 0);
            return false;
        }

        User32.MessageBox(nint.Zero, new string(buffer), "Success", 0);
        return true;
    }
}
