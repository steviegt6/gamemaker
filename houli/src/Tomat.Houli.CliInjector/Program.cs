using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.System.Memory;
using Windows.Win32.System.Threading;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Win32.SafeHandles;

namespace Tomat.Houli.CliInjector;

internal static class Program {
    internal static async Task<int> Main(string[] args) {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("This application is only supported on Windows.");

        return await new CliApplicationBuilder().AddCommandsFromThisAssembly().Build().RunAsync(args);
    }

    [SupportedOSPlatform("windows5.1.2600")]
    internal static unsafe void InjectIntoProcess(string applicationPath, string dllPath) {
        // TODO: Add preload support.
        var process = Process.Start(applicationPath);

        var processHandle = new SafeProcessHandle(process.Handle, true);
        var dllPathLength = (dllPath.Length + 1 /*null terminator*/) * Marshal.SizeOf<char>();

        var pLoadLib = PInvoke.GetProcAddress(PInvoke.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
        var pDllPath = PInvoke.VirtualAllocEx(
            processHandle,
            nint.Zero.ToPointer(),
            (nuint)dllPathLength,
            VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT
          | VIRTUAL_ALLOCATION_TYPE.MEM_RESERVE,
            PAGE_PROTECTION_FLAGS.PAGE_READWRITE
        );

        PInvoke.WriteProcessMemory(
            processHandle,
            pDllPath,
            GCHandle.Alloc(Encoding.Default.GetBytes(dllPath), GCHandleType.Pinned).AddrOfPinnedObject().ToPointer(),
            (nuint)dllPathLength,
            null
        );

        PInvoke.CreateRemoteThread(
            processHandle,
            null,
            nuint.Zero,
            pLoadLib.CreateDelegate<LPTHREAD_START_ROUTINE>(),
            pDllPath,
            0,
            null
        );
    }
}

[Command]
public class MainCommand : ICommand {
    [CommandParameter(0, Description = "Application path.")]
    public required string ApplicationPath { get; set; }

    [CommandParameter(1, Description = "DLL path.")]
    public required string DllPath { get; set; }

    [SupportedOSPlatform("windows5.1.2600")]
    public ValueTask ExecuteAsync(IConsole console) {
        if (!File.Exists(ApplicationPath))
            throw new FileNotFoundException("Application path does not exist.", ApplicationPath);

        if (!File.Exists(DllPath))
            throw new FileNotFoundException("DLL path does not exist.", DllPath);

        Directory.SetCurrentDirectory(Path.GetDirectoryName(ApplicationPath)!);
        Program.InjectIntoProcess(ApplicationPath, DllPath);
        return default;
    }
}
