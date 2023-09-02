using System;
using MinHook;
using Tomat.GameBreaker.API.Hooking;

namespace Tomat.GameBreaker.ManagedHost.Hooking;

/// <summary>
///     Provides a hook service using MinHook.
/// </summary>
public sealed class MinHookHookService : IHookService {
    private readonly HookEngine hookEngine = new();

    public TDelegate CreateHook<TDelegate>(string moduleName, string functionName, TDelegate callback) where TDelegate : Delegate {
        var orig = hookEngine.CreateHook(moduleName, functionName, callback);
        hookEngine.EnableHook(orig);
        return orig;
    }

    public TDelegate CreateHook<TDelegate>(nint address, TDelegate callback) where TDelegate : Delegate {
        var orig = hookEngine.CreateHook(address, callback);
        hookEngine.EnableHook(orig);
        return orig;
    }
}
