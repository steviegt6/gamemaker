// ReSharper disable InconsistentNaming

using System;

namespace Tomat.GameMaker.IFF;

/// <summary>
///     Well-known GameMaker versions.
///     <br />
///     These versions are explicitly used by us for determining chunk and model
///     behavior.
/// </summary>
public static class WellKnownVersion {
    public static readonly Version DEFAULT       = NormalizeVersion(new Version(1, 0, 0, 0));
    public static readonly Version GM_1_0_0_9999 = NormalizeVersion(new Version(1, 0, 0, 9999));
    public static readonly Version GM_2_0_0_0    = NormalizeVersion(new Version(2, 0, 0, 0));
    public static readonly Version GM_2_2_2_302  = NormalizeVersion(new Version(2, 2, 2, 302));
    public static readonly Version GM_2_3_0_0    = NormalizeVersion(new Version(2, 3, 0, 0));
    public static readonly Version GM_2_3_1_0    = NormalizeVersion(new Version(2, 2, 1, 0));
    public static readonly Version GM_2_3_2_0    = NormalizeVersion(new Version(2, 3, 2, 0));
    public static readonly Version GM_2_3_6_0    = NormalizeVersion(new Version(2, 3, 6, 0));
    public static readonly Version GM_2022_1     = NormalizeVersion(new Version(2022, 1));
    public static readonly Version GM_2022_2     = NormalizeVersion(new Version(2022, 2));
    public static readonly Version GM_2022_3     = NormalizeVersion(new Version(2022, 3));
    public static readonly Version GM_2022_5     = NormalizeVersion(new Version(2022, 5));
    public static readonly Version GM_2022_6     = NormalizeVersion(new Version(2022, 6));
    public static readonly Version GM_2022_8     = NormalizeVersion(new Version(2022, 8));
    public static readonly Version GM_2022_9     = NormalizeVersion(new Version(2022, 9));
    public static readonly Version GM_2023_1     = NormalizeVersion(new Version(2023, 1));
    public static readonly Version GM_2023_2     = NormalizeVersion(new Version(2023, 2));
    public static readonly Version GM_2023_4     = NormalizeVersion(new Version(2023, 4));

    internal static Version NormalizeVersion(Version version) {
        return new Version(
            version.Major == -1 ? 0 : version.Major,
            version.Minor == -1 ? 0 : version.Minor,
            version.Build == -1 ? 0 : version.Build,
            version.Revision == -1 ? 0 : version.Revision
        );
    }
}
