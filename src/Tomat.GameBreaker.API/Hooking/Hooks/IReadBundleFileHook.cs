using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface IReadBundleFileHook : IHook<IReadBundleFileHook.Delegate> {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    public unsafe delegate string Delegate([MarshalAs(UnmanagedType.LPWStr)] string a1, uint* a2);
}
