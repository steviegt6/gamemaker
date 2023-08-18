using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks; 

public class EndSceneHook : AbstractHook {
    public delegate int Delegate(nint @this);
    
    public override void CreateHook(HookEngine engine) {
        throw new System.NotImplementedException();
    }
}
