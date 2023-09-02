namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface ICodeExecuteHook : IHook<ICodeExecuteHook.Delegate> {
    public delegate bool Delegate(nint pSelf, nint pOther, nint code, nint res, int flags);
}
