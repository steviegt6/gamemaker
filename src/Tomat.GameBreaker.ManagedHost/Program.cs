using System;
using System.Runtime.InteropServices;
using Tomat.GameBreaker.ManagedHost.Utilities;

namespace Tomat.GameBreaker.ManagedHost;

internal static class Program {
    [UnmanagedCallersOnly]
    internal static unsafe void Main(short* cwd) {
        Console.WriteLine($"hi, {NativeUtil.ReadWCharPtr(cwd)}");
    }
}
