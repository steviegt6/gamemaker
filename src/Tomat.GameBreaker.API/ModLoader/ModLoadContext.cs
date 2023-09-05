using System.Runtime.Loader;

namespace Tomat.GameBreaker.API.ModLoader;

// TODO: How do we want to support unloading managed DLLs? Do we?
// ^ To elaborate, mods can still load them manually, but ALCs provide an API
// to resolve from as well, which is convenient!

/// <summary>
///     An <see cref="AssemblyLoadContext"/> for a mod.
/// </summary>
public abstract class ModLoadContext : AssemblyLoadContext {
    // TODO: Do we want to allow unloading these contexts? Use case? Please?
    protected ModLoadContext(string name) : base(name) { }

    /// <summary>
    ///     Registers the given <paramref name="mod"/> as the mod which this
    ///     context is for.
    /// </summary>
    /// <param name="mod">The mod to register.</param>
    /// <remarks>
    ///     This is done so that this context may load the assembly associated
    ///     with the <paramref name="mod"/> upon request, along with any other
    ///     assemblies which the mod may depend on.
    ///     <br />
    ///     This is because of how dependencies are used; see
    ///     <see cref="RegisterDependency"/> for more information.
    /// </remarks>
    /// <seealso cref="RegisterDependency"/>
    public abstract void RegisterSelf(IMod mod);

    /// <summary>
    ///     Registers the given <paramref name="dependency"/> as a dependency
    ///     of the mod which this context is for.
    /// </summary>
    /// <param name="dependency">
    ///     The dependency to register.
    /// </param>
    /// <remarks>
    ///     When a requested assembly is not found within this context, the
    ///     contexts within the <paramref name="dependency"/> mods are used.
    /// </remarks>
    public abstract void RegisterDependency(IMod dependency);
}
