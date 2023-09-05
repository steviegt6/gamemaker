namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface IReadBundleFileHook : IHook<IReadBundleFileHook.Delegate> {
    public unsafe delegate byte* Delegate(byte* a1, nuint* a2);
}
