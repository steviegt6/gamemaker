using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tomat.Houli.Native.Import;
using Tomat.Houli.Native.Util;

namespace Tomat.Houli.Native;

internal static class Program {
    private const int max_path = 256;
    public const int DLL_PROCESS_ATTACH = 1;

    private static nint initFuncPtr;
    private static nint delegateFuncPtr;
    private static nint closeFuncPtr;
    private static nint setPropFuncPtr;

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
            
            if (initFuncPtr == nint.Zero) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_initialize_for_runtime_config, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }
            
            if (delegateFuncPtr == nint.Zero) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_get_runtime_delegate, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }
            
            if (closeFuncPtr == nint.Zero) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_close, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }
            
            if (setPropFuncPtr == nint.Zero) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_set_runtime_property_value, please check the console output for errors!", "Houli Early Init Error", 0);
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
            Console.WriteLine($"Failed to get hostfxr path with error code: {rc:x8}");
            User32.MessageBox(nint.Zero, $"Failed to get hostfxr path with error code: {rc:x8}", "Houli Early Init Error", 0);
            return false;
        }

        try {
            var lib = LoadLibrary(new string(buffer));

            User32.MessageBox(nint.Zero, $"Loaded hostfxr {lib:x8}", "Houli Early Init", 0);

            initFuncPtr = GetExport(lib, "hostfxr_initialize_for_runtime_config");
            delegateFuncPtr = GetExport(lib, "hostfxr_get_runtime_delegate");
            closeFuncPtr = GetExport(lib, "hostfxr_close");
            setPropFuncPtr = GetExport(lib, "hostfxr_set_runtime_property_value");

            return true;
        }
        catch (Exception e) {
            Console.WriteLine($"Failed to load hostfxr with exception: {e}");
            User32.MessageBox(nint.Zero, $"Failed to load hostfxr with exception: {e}", "Houli Early Init Error", 0);
            throw;
        }
    }

    private static nint LoadLibrary(string path) {
        var handle = Kernel32.LoadLibrary(path);

        if (handle == nint.Zero)
            throw new Exception($"Failed to load library: {path}");

        return handle;
    }

    private static nint GetExport(nint h, string name) {
        var ptr = Kernel32.GetProcAddress(h, name);

        if (ptr == nint.Zero)
            throw new Exception($"Failed to get export: {name}");

        return ptr;
    }
}
