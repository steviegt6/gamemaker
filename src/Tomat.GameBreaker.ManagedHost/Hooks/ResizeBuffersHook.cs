using MinHook;

namespace Tomat.GameBreaker.ManagedHost.Hooks;

public class ResizeBuffersHook : AbstractHook {
    public delegate int Delegate(nint @this, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags);

    public override void CreateHook(HookEngine engine) {
        throw new System.NotImplementedException();
    }
}
