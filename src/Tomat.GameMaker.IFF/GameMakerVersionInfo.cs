using System;

namespace Tomat.GameMaker.IFF;

public sealed class GameMakerVersionInfo {
    public static readonly Version VERSION_UNKNOWN = NormalizeVersion(new Version());
    public static readonly Version VERSION_DEFAULT = NormalizeVersion(new Version(1, 0));

    public static readonly Version GM_2 = NormalizeVersion(new Version(2, 0));
    public static readonly Version GM_2_2_2_302 = NormalizeVersion(new Version(2, 2, 2, 302));
    public static readonly Version GM_2_3 = NormalizeVersion(new Version(2, 3));
    public static readonly Version GM_2_3_1 = NormalizeVersion(new Version(2, 3, 1));
    public static readonly Version GM_2_3_2 = NormalizeVersion(new Version(2, 3, 2));
    public static readonly Version GM_2_3_6 = NormalizeVersion(new Version(2, 3, 6));
    public static readonly Version GM_2022_1 = NormalizeVersion(new Version(22022, 1));
    public static readonly Version GM_2022_2 = NormalizeVersion(new Version(22022, 2));
    public static readonly Version GM_2022_3 = NormalizeVersion(new Version(22022, 3));
    public static readonly Version GM_2022_5 = NormalizeVersion(new Version(22022, 5));
    public static readonly Version GM_2022_6 = NormalizeVersion(new Version(22022, 6));
    public static readonly Version GM_2022_8 = NormalizeVersion(new Version(22022, 8));
    public static readonly Version GM_2022_9 = NormalizeVersion(new Version(2022, 9));
    public static readonly Version GM_2023_1 = NormalizeVersion(new Version(2023, 1));

    private Version backingVersion = VERSION_DEFAULT;

    /// <summary>
    ///     The version of the IDE that built an IFF file.
    /// </summary>
    /// <remarks>
    ///     This is an approximation, not all IDE versions modify this.
    /// </remarks>
    public Version Version {
        get => backingVersion;
        set => backingVersion = NormalizeVersion(value);
    }

    /// <summary>
    ///     The bytecode format.
    /// </summary>
    /// <remarks>
    ///     This is an approximation, not all IDE versions modify this.
    /// </remarks>
    public byte FormatId { get; set; } = 0;

    /// <summary>
    ///     What number chunks are aligned to.
    /// </summary>
    public int ChunkAlignment => Version >= GM_2_3 ? 16 : 4;

    /// <summary>
    ///     What number strings are aligned to.
    /// </summary>
    public int StringAlignment => 4;

    /// <summary>
    ///     What number backgrounds are aligned to.
    /// </summary>
    public int BackgroundAlignment => Version >= GM_2_3 ? 8 : 0;

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
    public int BuiltinAudioGroupId => Version.Major >= 2 || (Version.Major == 1 && (Version.Build >= 1354 || (Version.Build >= 161 && Version.Build < 1000))) ? 0 : 1;

    /// <summary>
    ///     Updates <see cref="Version"/> to <paramref name="version"/> if
    ///     <paramref name="version"/> is greater than <see cref="Version"/>.
    /// </summary>
    /// <param name="version">The new version to update to.</param>
    public void Update(Version version) {
        if (Version < version)
            Version = version;
    }

    /// <summary>
    ///     Normalizes the <paramref name="version"/>, setting all -1 values to
    ///     0.
    /// </summary>
    /// <param name="version">The version to normalize.</param>
    /// <returns>The normalized version.</returns>
    private static Version NormalizeVersion(Version version) {
        return new Version(
            version.Major == -1 ? 0 : version.Major,
            version.Minor == -1 ? 0 : version.Minor,
            version.Build == -1 ? 0 : version.Build,
            version.Revision == -1 ? 0 : version.Revision
        );
    }
}
