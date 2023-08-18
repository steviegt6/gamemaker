using System;
using System.Runtime.InteropServices;
using Tomat.GameBreaker.ManagedHost.Hooks;

namespace Tomat.GameBreaker.ManagedHost;

internal static class Program {
    [UnmanagedCallersOnly]
    internal static unsafe void Main(short* cwd) {
        // Console.WriteLine($"hi, {NativeUtil.ReadWCharPtr(cwd)}");

        Console.WriteLine("Initializing hooks...");
        HookManager.Initialize();
    }
}
