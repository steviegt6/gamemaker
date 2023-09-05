using System.Reflection;

namespace Tomat.GameBreaker.API.ModLoader;

public interface IAssemblyResolver {
    void AddDependency(IAssemblyResolver dependency);

    Assembly? ResolveAssembly(AssemblyName assemblyName);
}
