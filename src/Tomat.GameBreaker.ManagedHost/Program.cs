using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.Platform;
using Tomat.GameBreaker.ManagedHost.Utilities;

namespace Tomat.GameBreaker.ManagedHost;

internal static class Program {
    [UnmanagedCallersOnly]
    internal static unsafe void Main(short* pCwd, short* pDllPath, int major, int minor, int patch) {
        Debugger.Launch();
        var cwd = NativeUtil.ReadWCharPtr(pCwd);
        var dllPath = NativeUtil.ReadWCharPtr(pDllPath);
        Console.WriteLine($"Managed context recognizing current directory: '{cwd}'");
        Console.WriteLine($"Managed context recognizing DLL path: '{dllPath}'");

        Console.WriteLine("Setting up game...");
        Game game = new ManagedHostGame(cwd);

        var proc = Process.GetCurrentProcess();
        var platform = game.ServiceProvider.ExpectService<IPlatformService>();
        if (!platform.IsSuspended(proc))
            platform.Restart(proc, new[] { dllPath });

        game.Initialize();
        platform.Resume(proc);
    }
}
