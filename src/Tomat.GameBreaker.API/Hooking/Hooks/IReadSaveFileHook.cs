namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface IReadSaveFileHook : IHook<IReadSaveFileHook.Delegate> {
    public unsafe delegate void* Delegate(byte* a1, nuint* a2, void* a3);
}
