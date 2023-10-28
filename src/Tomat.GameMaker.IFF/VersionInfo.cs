using System;

namespace Tomat.GameMaker.IFF;

/// <summary>
///     Information about the version of the IFF file and IDE that created it.
/// </summary>
public sealed class VersionInfo {
    private Version backingVersion = WellKnownVersion.DEFAULT;

    /// <summary>
    ///     The version of the IDE that built this IFF file.
    /// </summary>
    /// <remarks>
    ///     This is an approximation as not all IDE versions modify this.
    /// </remarks>
    public Version Version {
        get => backingVersion;
        set => backingVersion = WellKnownVersion.NormalizeVersion(value);
    }

    /// <summary>
    ///     The bytecode format.
    /// </summary>
    /// <remarks>
    ///     This is an approximation as not all IDE versions modify this.
    /// </remarks>
    public byte FormatId { get; set; }

    public int ChunkAlignment => IsAtLeast(GM_2_3_0_0) ? 16 : 4;

    public int StringAlignment => 4;

    public int BackgroundAlignment => IsAtLeast(GM_2_3_0_0) ? 8 : 0;

    /// <summary>
    ///     Whether rooms and objects use pre-create events.
    /// </summary>
    public bool RoomsAndObjectsUsePreCreate { get; set; }

    /// <summary>
    ///     Whether some unknown variables in the <c>VARI</c> chunk file have
    ///     different values. Only used if <see cref="FormatId"/> is greater
    ///     than or equal to 14.
    /// </summary>
    public bool DifferentVarCounts { get; set; }

    /// <summary>
    ///     Whether the IFF file uses option flags in the <c>OPTN</c> chunk.
    /// </summary>
    public bool OptionBitflag { get; set; }

    /// <summary>
    ///     Whether the IFF file was run from the IDE.
    /// </summary>
    public bool WasRunFromIde { get; set; }

    /// <summary>
    ///     Whether the virtual machine short-circuits logical AND/OR operators.
    /// </summary>
    public bool ShortCircuit { get; set; }

    /// <summary>
    ///     The ID of the built-in, main audio group.
    /// </summary>
    public int BuiltinAudioGroupId => IsAtLeast(GM_2_0_0_0) || Version is { Major: 1, Build: >= 1354 or >= 161 and < 1000 } ? 0 : 1;

    /// <summary>
    ///     Updates the version to the specified version if it is newer.
    /// </summary>
    /// <param name="version">The version to update to.</param>
    public void UpdateTo(Version version) {
        if (Version < version)
            Version = version;
    }

    /// <summary>
    ///     Whether this version is at least the specified version.
    /// </summary>
    /// <param name="version">The version to compare against.</param>
    /// <returns>
    ///     Whether this version is at least the specified version.
    /// </returns>
    public bool IsAtLeast(Version version) {
        return Version >= version;
    }
}
