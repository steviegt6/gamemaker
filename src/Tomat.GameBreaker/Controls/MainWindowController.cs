namespace Tomat.GameBreaker.Controls;

public record MainWindowController(
    // AboutDialog options.
    string? ApplicationIcon,
    string? ApplicationName,
    string[] Artists,
    string? Comments,
    string? Copyright,
    string? DebugInfo,
    string? DebugInfoFilename,
    string[] Designers,
    string? DeveloperName,
    string[] Developers,
    string[] Documenters,
    string? IssueUrl,
    string? License,
    Gtk.License LicenseType,
    string? ReleaseNotes,
    string? ReleaseNotesVersion,
    string? SupportUrl,
    string? TranslatorCredits,
    string? Version,
    string? Website,

    // Window display options.
    string WindowTitle,
    int DefaultWidth,
    int DefaultHeight
);
