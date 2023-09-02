using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.ManagedHost.Hooking;

namespace Tomat.GameBreaker.ManagedHost;

internal sealed class ManagedHostGame : Game {
    public override IServiceProvider ServiceProvider { get; }

    public ManagedHostGame() {
        ServiceProvider = CreateServiceProvider();
    }

    private IServiceProvider CreateServiceProvider() {
        var provider = new DefaultServiceProvider();
        provider.RegisterService(this);
        provider.RegisterService<IHookService>(new MinHookHookService());
        return provider;
    }
}
