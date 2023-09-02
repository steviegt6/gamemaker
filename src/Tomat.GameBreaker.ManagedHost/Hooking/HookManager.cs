using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.Hooking.Hooks;
using Tomat.GameBreaker.ManagedHost.Hooking.Hooks;

namespace Tomat.GameBreaker.ManagedHost.Hooking;

internal static class HookManager {
    public static void CreateHooksForGame(Game game) {
        var serviceProvider = game.ServiceProvider;
        var hookService = serviceProvider.ExpectService<IHookService>();

        serviceProvider.RegisterService<IMessageBoxWHook>(ExecuteHook(new MessageBoxWHook(), hookService));
        serviceProvider.RegisterService<IReadBundleFileHook>(ExecuteHook(new ReadBundleFileHook(), hookService));
    }

    private static THook ExecuteHook<THook>(THook hook, IHookService hookService) where THook : IHook {
        hook.CreateHook(hookService);
        return hook;
    }
}
