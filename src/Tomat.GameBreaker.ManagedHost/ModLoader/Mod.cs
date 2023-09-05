using System.Reflection;
using Tomat.GameBreaker.API.ModLoader;

namespace Tomat.GameBreaker.ManagedHost.ModLoader; 

internal sealed class Mod : IMod {
    public IModMetadata Metadata { get; set; }

    public ModLoadContext LoadContext { get; set; }

    public IAssemblyResolver AssemblyResolver { get; set; }

    public Assembly Assembly { get; set; }
    
    public Mod(IModMetadata metadata, ModLoadContext loadContext, IAssemblyResolver assemblyResolver, Assembly assembly) {
        Metadata = metadata;
        LoadContext = loadContext;
        AssemblyResolver = assemblyResolver;
        Assembly = assembly;
    }
}
