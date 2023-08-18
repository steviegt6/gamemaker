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
        return original!(hWnd, "HOOKED BY GAMEBREAKER: " + lpText, lpCaption, uType);
    }
}
