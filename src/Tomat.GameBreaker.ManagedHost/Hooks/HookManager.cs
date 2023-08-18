using System.Runtime.InteropServices;
using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks;

public static partial class HookManager {
    private static HookEngine? hookEngine;

    public static HookEngine HookEngine => hookEngine ??= new HookEngine();

    public static void Initialize() {
        new MessageBoxWHook().CreateHook(HookEngine);

        HookEngine.EnableHooks();

        MessageBoxW(nint.Zero, "hi", "caption", 0);
    }

    [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    public static partial int MessageBoxW(nint hWnd, string text, string caption, uint type);
}
