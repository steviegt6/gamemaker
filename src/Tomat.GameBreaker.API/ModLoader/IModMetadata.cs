using System;

namespace Tomat.GameBreaker.API.ModLoader; 

/// <summary>
///     Metadata for a mod.
/// </summary>
public interface IModMetadata {
    /// <summary>
    ///     The metadata version.
    /// </summary>
    int MetadataVersion { get; }
    
    /// <summary>
    ///     The unique name of the mod.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    ///     The version of the mod.
    /// </summary>
    Version Version { get; }
    
    /// <summary>
    ///     The dependencies of the mod.
    /// </summary>
    IModDependency[]? Dependencies { get; }
}
