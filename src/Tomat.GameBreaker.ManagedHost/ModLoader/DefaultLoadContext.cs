using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tomat.GameBreaker.API.ModLoader;

namespace Tomat.GameBreaker.ManagedHost.ModLoader;

internal sealed class DefaultLoadContext : ModLoadContext {
    private readonly Dictionary<string, IMod> dependencies = new();
    private IMod? self;

    public DefaultLoadContext(string name) : base(name) { }

    protected override Assembly? Load(AssemblyName assemblyName) {
        // This context isn't ready if the self mod hasn't been registered yet.
        if (self is null)
            return null;

        // See if we can just... load it?
        /*if (base.Load(assemblyName) is { } baseAssembly)
            return baseAssembly;*/
        
        if (All.ToList()[0].LoadFromAssemblyName(assemblyName) is { } magicAssembly)
            return magicAssembly;

        // TODO: Unsure of how necessary this is. Better safe than sorry?
        if (Default.LoadFromAssemblyName(assemblyName) is { } defaultAssembly)
            return defaultAssembly;

        // Actually resolve with our assembly resolver.
        if (self.AssemblyResolver.ResolveAssembly(assemblyName) is { } selfAssembly)
            return selfAssembly;

        // The assembly resolver above SHOULD handle attempting to resolve with
        // dependencies, but let's also do it here just to be safe!
        // This is the only reason we keep track of dependencies in load
        // contexts, so if we can guarantee this isn't needed, we can remove
        // the dependency tracking.
        foreach (var dependency in dependencies.Values) {
            if (dependency.AssemblyResolver.ResolveAssembly(assemblyName) is { } dependencyAssembly)
                return dependencyAssembly;
        }

        // The default is actually null, we call it earlier because I'm a little
        // silly. It doesn't matter too much -- hey, we might change the base
        // context some time... then who'll be laughing?
        return base.Load(assemblyName);
    }

    public override void RegisterSelf(IMod mod) {
        if (self is not null)
            throw new System.InvalidOperationException("Cannot register more than one mod as self.");

        self = mod;
    }

    public override void RegisterDependency(IMod dependency) {
        if (dependencies.ContainsKey(dependency.Metadata.Name))
            throw new System.InvalidOperationException("Cannot register more than one dependency with the same name.");

        dependencies.Add(dependency.Metadata.Name, dependency);
    }
}
