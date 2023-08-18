using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks; 

public class WindowProcHook : AbstractHook {
    public delegate int Delegate(nint hWnd, uint msg, nint w, nint l);
    
    public override void CreateHook(HookEngine engine) {
        throw new System.NotImplementedException();
    }
}
