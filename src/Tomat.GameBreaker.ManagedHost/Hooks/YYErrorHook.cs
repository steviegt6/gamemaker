using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks;

public class YYErrorHook : AbstractHook {
    // TODO: figure out better arg list handling
    public delegate void Delegate(nint pFormat, nint args);

    public override void CreateHook(HookEngine engine) {
        throw new System.NotImplementedException();
    }
}
