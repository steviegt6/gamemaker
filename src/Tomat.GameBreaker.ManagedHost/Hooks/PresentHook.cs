using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks;

public class PresentHook : AbstractHook {
    public delegate int Delegate(nint @this, uint sync, uint flags);

    public override void CreateHook(HookEngine engine) {
        throw new System.NotImplementedException();
    }
}
