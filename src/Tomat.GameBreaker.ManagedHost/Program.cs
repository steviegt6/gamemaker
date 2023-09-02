using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Tomat.GameBreaker.ManagedHost.Utilities;

namespace Tomat.GameBreaker.ManagedHost;

internal static class Program {
    [UnmanagedCallersOnly]
    [SupportedOSPlatform("windows5.1.2600")]
    internal static unsafe void Main(short* cwd) {
        Console.WriteLine($"Managed context recognizing current directory: '{NativeUtil.ReadWCharPtr(cwd)}'");

        Console.WriteLine("Setting up game...");
        var game = new ManagedHostGame();
        game.Initialize();
        PInvoke.DebugActiveProcessStop((uint)Environment.ProcessId);
        game.WaitForProcessExit(Environment.ProcessId);
    }
}
