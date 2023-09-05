using System.Reflection;

namespace Tomat.GameBreaker.API.ModLoader;

/// <summary>
///     Represents a mod at runtime.
/// </summary>
public interface IMod {
    /// <summary>
    ///     The metadata for this mod.
    /// </summary>
    IModMetadata Metadata { get; set; }

    /// <summary>
    ///     The load context for this mod.
    /// </summary>
    ModLoadContext LoadContext { get; set; }

    /// <summary>
    ///     The assembly resolver for this mod.
    /// </summary>
    IAssemblyResolver AssemblyResolver { get; set; }

    /// <summary>
    ///     The mod's loaded assembly.
    /// </summary>
    Assembly Assembly { get; set; }
}
