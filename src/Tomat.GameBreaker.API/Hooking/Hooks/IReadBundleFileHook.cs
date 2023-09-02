using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface IReadBundleFileHook : IHook<IReadBundleFileHook.Delegate> {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    public delegate string Delegate([MarshalAs(UnmanagedType.LPWStr)] string a1, nint /* uint* */ a2);
}
