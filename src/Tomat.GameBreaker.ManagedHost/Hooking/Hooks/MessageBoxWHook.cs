using System;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.Hooking.Hooks;

namespace Tomat.GameBreaker.ManagedHost.Hooking.Hooks;

internal sealed class MessageBoxWHook : IMessageBoxWHook {
    public IMessageBoxWHook.Delegate? Original { get; set; }

    public void CreateHook(IHookService hookService) {
        this.CreateHook(hookService, "user32.dll", "MessageBoxW", Hook);
    }

    private int Hook(nint hWnd, string lpText, string lpCation, int uType) {
        if (!lpText.StartsWith("Win32 function failed"))
            return Original!(hWnd, lpText, lpCation, uType);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"Caught Win32 'function failed' message: [{lpCation}] {lpText}");
        Console.ResetColor();
        return 0;
    }
}
