using System;
using System.Runtime.InteropServices;
using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks;

public class MessageBoxWHook : AbstractHook {
    public delegate int Delegate(nint hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpText, [MarshalAs(UnmanagedType.LPWStr)] string lpCaption, int uType);

    private Delegate? original;

    public override void CreateHook(HookEngine engine) {
        original = engine.CreateHook("user32.dll", "MessageBoxW", new Delegate(DelegateHook));
    }

    private int DelegateHook(nint hWnd, string lpText, string lpCaption, int uType) {
        if (!lpText.StartsWith("Win32 function failed"))
            return original!(hWnd, lpText, lpCaption, uType);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"Caught Win32 function failed message: [{lpCaption}] {lpText}");
        Console.ResetColor();
        return 0;
    }
}
