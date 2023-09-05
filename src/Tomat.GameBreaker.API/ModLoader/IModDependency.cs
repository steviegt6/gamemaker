using System;

namespace Tomat.GameBreaker.API.ModLoader; 

/// <summary>
///     Represents a dependency of a mod.
/// </summary>
public interface IModDependency {
    /// <summary>
    ///     The name of the dependency.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    ///     The version of the dependency.
    /// </summary>
    Version Version { get; }
}
