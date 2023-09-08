using System;
using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Hooking.Hooks;

public interface IHook {
    void CreateHook(IHookService hookService);
}

public interface IHook<TDelegate> : IHook where TDelegate : Delegate {
    TDelegate? Original { get; set; }
}

public static class HookExtensions {
    public static void CreateHook<TDelegate>(this IHook<TDelegate> hook, IHookService hookService, string moduleName, string functionName, TDelegate callback) where TDelegate : Delegate {
        GCHandle.Alloc(callback);
        hook.Original = hookService.CreateHook(moduleName, functionName, callback);
    }

    public static void CreateHook<TDelegate>(this IHook<TDelegate> hook, IHookService hookService, nint address, TDelegate callback) where TDelegate : Delegate {
        GCHandle.Alloc(callback);
        hook.Original = hookService.CreateHook(address, callback);
    }
}
