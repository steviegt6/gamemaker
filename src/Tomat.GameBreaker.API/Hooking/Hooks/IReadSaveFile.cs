using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface IReadSaveFile {
    public delegate nint Delegate([MarshalAs(UnmanagedType.LPStr)] string /* LPCSTR */ lpMultiByteStr, nint /* uint* */ a2);
}
