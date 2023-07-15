namespace Tomat.GameBreaker.Controls;

public record MainWindowController(
    // AboutDialog options.
    string[] Artists,
    string[] Authors,
    string Comments,
    string Copyright,
    string[] Documenters,
    string License,
    Gtk.License LicenseType,
    Gdk.Paintable Logo,
    string? LogoIconName,
    string? ProgramName,
    string? SystemInformation,
    string? TranslatorCredits,
    string? Version,
    string? Website,
    string? WebsiteLabel,
    bool WrapLicense,

    // Window display options.
    string WindowTitle,
    int DefaultWidth,
    int DefaultHeight
);
