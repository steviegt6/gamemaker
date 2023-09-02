using System;
using System.Runtime.InteropServices;
using Tomat.GameBreaker.ManagedHost.Utilities;

namespace Tomat.GameBreaker.ManagedHost;

internal static class Program {
    [UnmanagedCallersOnly]
    internal static unsafe void Main(short* cwd) {
        Console.WriteLine($"Managed context recognizing current directory: '{NativeUtil.ReadWCharPtr(cwd)}'");

        Console.WriteLine("Setting up game...");
        var game = new ManagedHostGame();
        game.Initialize();
        game.WaitForProcessExit(Environment.ProcessId);
    }
}
