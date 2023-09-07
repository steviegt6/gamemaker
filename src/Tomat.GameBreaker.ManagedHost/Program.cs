using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.System.Threading;
using Tomat.GameBreaker.API;
using Tomat.GameBreaker.ManagedHost.Utilities;

namespace Tomat.GameBreaker.ManagedHost;

internal static class Program {
    [UnmanagedCallersOnly]
    [SupportedOSPlatform("windows5.1.2600")]
    internal static unsafe void Main(short* cwd) {
        Debugger.Launch();
        var dir = NativeUtil.ReadWCharPtr(cwd);
        Console.WriteLine($"Managed context recognizing current directory: '{dir}'");

        Console.WriteLine("Setting up game...");
        Game game = new ManagedHostGame(dir);
        game.Initialize();
        ResumeProcess();
    }

    /*[LibraryImport("dbgcore.dll", EntryPoint = "resume_process")]
    private static partial void ResumeProcess();*/

    [SupportedOSPlatform("windows5.1.2600")]
    private static void ResumeProcess() {
        var process = Process.GetCurrentProcess();

        foreach (ProcessThread thread in process.Threads) {
            var threadHandle = PInvoke.OpenThread(THREAD_ACCESS_RIGHTS.THREAD_SUSPEND_RESUME, false, (uint)thread.Id);
            if (threadHandle.IsNull)
                continue;

            uint suspendCount;

            do {
                suspendCount = PInvoke.ResumeThread(threadHandle);
            }
            while (suspendCount > 0);

            PInvoke.CloseHandle(threadHandle);
        }
    }
}
