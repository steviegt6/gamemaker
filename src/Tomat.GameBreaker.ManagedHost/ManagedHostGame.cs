using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.PatternSearching;
using Tomat.GameBreaker.ManagedHost.Hooking;
using Tomat.GameBreaker.ManagedHost.PatternSearching;

namespace Tomat.GameBreaker.ManagedHost;

internal sealed class ManagedHostGame : Game {
    public override IServiceProvider ServiceProvider { get; }

    public ManagedHostGame() {
        ServiceProvider = CreateServiceProvider();
    }

    public override void Initialize() {
        HookManager.CreateHooksForGame(this);
    }

    private IServiceProvider CreateServiceProvider() {
        var provider = new DefaultServiceProvider();
        provider.RegisterService(this);
        provider.RegisterService<IPatternSearchService>(new PatternSearchService());
        provider.RegisterService<IHookService>(new MinHookHookService());
        return provider;
    }
}
