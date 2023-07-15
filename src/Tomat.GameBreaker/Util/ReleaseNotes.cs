using System;
using System.Collections.Generic;
using System.Text;

namespace Tomat.GameBreaker.Util;

internal static class ReleaseNotes {
    public const string RELEASE_NOTES_VERSION = "1.0.0.0";

    private static readonly Dictionary<string, string> RELEASE_NOTES_BY_VERSION = new() {
        { "1.0.0.0", release_notes_1_0_0_0 }
    };

    private static string? releaseNotes;

    private const string release_notes_1_0_0_0 = @"
    <p>Additions</p>
    <ul>
        <li>Test item!</li>
    </ul>
";

    public static string GetReleaseNotes() {
        if (releaseNotes is not null)
            return releaseNotes;

        return releaseNotes = RELEASE_NOTES_BY_VERSION[RELEASE_NOTES_VERSION];
    }
}
