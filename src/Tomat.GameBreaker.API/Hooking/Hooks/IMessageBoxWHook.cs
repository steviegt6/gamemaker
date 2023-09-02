using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface IMessageBoxWHook : IHook<IMessageBoxWHook.Delegate> {
    public delegate int Delegate(nint hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpText, [MarshalAs(UnmanagedType.LPWStr)] string lpCaption, int uType);
}
