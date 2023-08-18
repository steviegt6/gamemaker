using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks; 

public class DoCallScriptHook : AbstractHook {
    public delegate nint Delegate(nint pScript, int argc, nint pStackPointer, nint pVm, nint pLocals, nint pArguments);
    
    public override void CreateHook(HookEngine engine) {
        throw new System.NotImplementedException();
    }
}
