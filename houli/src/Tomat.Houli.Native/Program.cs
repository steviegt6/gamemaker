using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tomat.Houli.Native.Import;
using Tomat.Houli.Native.Util;

namespace Tomat.Houli.Native;

internal static class Program {
    private const int max_path = 256;
    public const int DLL_PROCESS_ATTACH = 1;

    private delegate void NativeEntryInitialize(nint pBaseDirectory);

    private static Hostfxr.HostfxrInitializeForRuntimeConfigFn? initFuncPtr;
    private static Hostfxr.HostfxrGetRuntimeDelegateFn? delegateFuncPtr;
    private static Hostfxr.HostfxrCloseFn? closeFuncPtr;
    private static Hostfxr.HostfxrSetRuntimePropertyFn? setPropFuncPtr;

    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new[] { typeof(CallConvStdcall) })]
    public static unsafe bool DllMain(nint hModule, uint ulReasonForCall, nint lpReserved) {
        if (ulReasonForCall == DLL_PROCESS_ATTACH) {
            // If anything fails, it's reported in the method. We can return
            // from there.
            if (!ConsoleWindow.Initialize())
                return false;

            Console.WriteLine($"Using {nameof(StringMarshalling)}: {Shared.STRING_MARSHALLING}");
            Console.WriteLine($"Using {nameof(UnmanagedType)}: {Shared.UNMANAGED_STRING_TYPE}");

            if (!LoadHostfxr()) {
                User32.MessageBox(nint.Zero, "Failed to lost hostfxr, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }

            if (initFuncPtr is null) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_initialize_for_runtime_config, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }

            if (delegateFuncPtr is null) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_get_runtime_delegate, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }

            if (closeFuncPtr is null) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_close, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }

            if (setPropFuncPtr is null) {
                User32.MessageBox(nint.Zero, "Failed to get hostfxr_set_runtime_property_value, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }

            // TODO: Config path and base directory.
            var loadAssemblyAndGetFunctionPointer = GetDotnetLoadAssembly(Path.Combine(".houli", "lib", "Tomat.Houli.API.runtimeconfig.json"), Directory.GetCurrentDirectory());

            if (loadAssemblyAndGetFunctionPointer is null) {
                User32.MessageBox(nint.Zero, "Failed to get dotnet_load_assembly_and_get_function_pointer, please check the console output for errors!", "Houli Early Init Error", 0);
                return false;
            }

            var ppInitialize = stackalloc nint[1];
            var rc = loadAssemblyAndGetFunctionPointer(
                Path.Combine(".houli", "lib", "Tomat.Houli.API.dll"),
                "Tomat.Houli.API.NativeEntry",
                "Initialize",
                -1, // UNMANAGEDCALLERSONLY_METHOD
                0, // null
                ppInitialize
            );

            var pInitialize = *ppInitialize;

            if (rc != 0 || pInitialize == 0) {
                Console.WriteLine($"Failed to get Initialize function pointer with error code: {rc:x8}");
                User32.MessageBox(nint.Zero, $"Failed to get Initialize function pointer with error code: {rc:x8}", "Houli Early Init Error", 0);
                return false;
            }

            Console.WriteLine();

            var currentDirectory = Directory.GetCurrentDirectory();

            fixed (char* pCurrentDirectory = currentDirectory) {
                ToDelegate<NativeEntryInitialize>(pInitialize)((nint)pCurrentDirectory);
            }
        }

        return true;
    }

    private static unsafe bool LoadHostfxr() {
        var buffer = new char[max_path];

        // No need to divide by char size unlike in native... probably?
        var bufferSize = (nint)buffer.Length;

        int rc;

        fixed (char* pBuffer = /*&buffer[0]*/ buffer) {
            rc = Nethost.GetHostfxrPath(pBuffer, &bufferSize, null);
        }

        if (rc != 0) {
            Console.WriteLine($"Failed to get hostfxr path with error code: {rc:x8}");
            Console.WriteLine($"{new string(buffer)}");
            User32.MessageBox(nint.Zero, $"Failed to get hostfxr path with error code: {rc:x8}", "Houli Early Init Error", 0);
            return false;
        }

        try {
            var lib = LoadLibrary(new string(buffer));

            User32.MessageBox(nint.Zero, $"Loaded hostfxr {lib:x8}", "Houli Early Init", 0);

            initFuncPtr = ToDelegate<Hostfxr.HostfxrInitializeForRuntimeConfigFn>(GetExport(lib, "hostfxr_initialize_for_runtime_config"));
            delegateFuncPtr = ToDelegate<Hostfxr.HostfxrGetRuntimeDelegateFn>(GetExport(lib, "hostfxr_get_runtime_delegate"));
            closeFuncPtr = ToDelegate<Hostfxr.HostfxrCloseFn>(GetExport(lib, "hostfxr_close"));
            setPropFuncPtr = ToDelegate<Hostfxr.HostfxrSetRuntimePropertyFn>(GetExport(lib, "hostfxr_set_runtime_property_value"));

            return true;
        }
        catch (Exception e) {
            Console.WriteLine($"Failed to load hostfxr with exception: {e}");
            User32.MessageBox(nint.Zero, $"Failed to load hostfxr with exception: {e}", "Houli Early Init Error", 0);
            throw;
        }
    }

    private static unsafe CoreclrDelegates.LoadAssemblyAndGetFunctionPointerFn? GetDotnetLoadAssembly(string configPath, string baseDirectory) {
        var rc = initFuncPtr!(configPath, null, out var hostfxrHandle);

        if (rc != 0 || hostfxrHandle == 0) {
            Console.WriteLine($"Failed to load assembly with error code: {rc:x8}");
            return null;
        }

        rc = delegateFuncPtr!(hostfxrHandle, Hostfxr.HostfxrDelegateType.HdtLoadAssemblyAndGetFunctionPointer, out var loadAssemblyGetAndFunctionPointer);

        if (rc != 0 || loadAssemblyGetAndFunctionPointer == 0) {
            Console.WriteLine($"Get delegate failed with error code: {rc:x8}");
            return null;
        }

        setPropFuncPtr!(hostfxrHandle, "APP_CONTEXT_BASE_DIRECTORY", baseDirectory);
        closeFuncPtr!(hostfxrHandle);

        return ToDelegate<CoreclrDelegates.LoadAssemblyAndGetFunctionPointerFn>(loadAssemblyGetAndFunctionPointer);
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

    private static T ToDelegate<T>(nint ptr) where T : Delegate {
        return Marshal.GetDelegateForFunctionPointer<T>(ptr);
    }
}
