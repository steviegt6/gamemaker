using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.FileModification;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.ModLoader;
using Tomat.GameBreaker.API.PatternSearching;
using Tomat.GameBreaker.API.Platform;
using Tomat.GameBreaker.ManagedHost.DefaultMod;
using Tomat.GameBreaker.ManagedHost.FileModification;
using Tomat.GameBreaker.ManagedHost.Hooking;
using Tomat.GameBreaker.ManagedHost.ModLoader;
using Tomat.GameBreaker.ManagedHost.PatternSearching;
using Tomat.GameBreaker.ManagedHost.Platform;
using IServiceProvider = Tomat.GameBreaker.API.DependencyInjection.IServiceProvider;

namespace Tomat.GameBreaker.ManagedHost;

internal sealed class ManagedHostGame : Game {
    public override IServiceProvider ServiceProvider { get; }

    private readonly string dir;

    public ManagedHostGame(string dir) {
        this.dir = dir;
        ServiceProvider = CreateServiceProvider();
    }

    public override void Initialize() {
        HookManager.CreateHooksForGame(this);

        var modsDir = Path.Combine(dir, "mods");
        Directory.CreateDirectory(modsDir);

        var modLoader = ServiceProvider.ExpectService<IModLoader>();
        var resolved = modLoader.ResolveModsFromDirectory(modsDir);
        var modified = resolved.ToList();
        modified.Add(CreateDefaultMod());
        modLoader.SortAndRegisterMods(modified);
        modLoader.LoadMods(this);
    }

    private IServiceProvider CreateServiceProvider() {
        var provider = new DefaultServiceProvider();
        provider.RegisterService(this);
        provider.RegisterService(MakePlatformServiceForCurrentPlatform());
        provider.RegisterService<IPatternSearchService>(new PatternSearchService());
        provider.RegisterService<IHookService>(new MinHookHookService());
        provider.RegisterService<IModLoader>(new DefaultModLoader());
        provider.RegisterService<IFileModifierService>(new FileModifierService());
        return provider;
    }

    private static IMod CreateDefaultMod() {
        var modMetadata = new DefaultModMetadata();
        var asm = typeof(Program).Assembly;
        var loadContext = new DefaultLoadContext(modMetadata.Name);
        var mod = new Mod(modMetadata, loadContext, new AssemblyResolver(AssemblyLoadContext.Default, asm, Path.GetDirectoryName(asm.Location)!), asm);
        loadContext.RegisterSelf(mod);
        return mod;
    }

    private static IPlatformService MakePlatformServiceForCurrentPlatform() {
        if (OperatingSystem.IsWindows())
            return new WindowsPlatform(Environment.Is64BitProcess);

        if (OperatingSystem.IsMacOS())
            return new OsxPlatform(Environment.Is64BitProcess);

        if (OperatingSystem.IsLinux())
            return new LinuxPlatform(Environment.Is64BitProcess);

        throw new PlatformNotSupportedException();
    }
}
