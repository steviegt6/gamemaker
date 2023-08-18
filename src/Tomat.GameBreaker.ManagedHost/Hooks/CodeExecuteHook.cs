using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks; 

public class CodeExecuteHook : AbstractHook {
    public delegate bool Delegate(nint pSelf, nint pOther, nint code, nint res, int flags);

    public override void CreateHook(HookEngine engine) {
        throw new System.NotImplementedException();
    }
}
