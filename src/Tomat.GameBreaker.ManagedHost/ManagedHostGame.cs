using System;
using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.PatternSearching;
using Tomat.GameBreaker.API.Platform;
using Tomat.GameBreaker.ManagedHost.Hooking;
using Tomat.GameBreaker.ManagedHost.PatternSearching;
using Tomat.GameBreaker.ManagedHost.Platform;
using IServiceProvider = Tomat.GameBreaker.API.DependencyInjection.IServiceProvider;

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
        provider.RegisterService(MakePlatformServiceForCurrentPlatform());
        provider.RegisterService<IPatternSearchService>(new PatternSearchService());
        provider.RegisterService<IHookService>(new MinHookHookService());
        return provider;
    }

    private static IPlatformService MakePlatformServiceForCurrentPlatform() {
        if (OperatingSystem.IsWindows())
            return Environment.Is64BitProcess ? new WindowsPlatform64() : new WindowsPlatform32();

        if (OperatingSystem.IsMacOS())
            return Environment.Is64BitProcess ? new OsxPlatform64() : new OsxPlatform32();

        if (OperatingSystem.IsLinux())
            return Environment.Is64BitProcess ? new LinuxPlatform64() : new LinuxPlatform32();

        throw new PlatformNotSupportedException();
    }
}
