namespace Tomat.Houli.Launcher.Controls;

public sealed record MainWindowController(
    // AboutDialog
    string[] Artists,
    string[] Authors,
    string? Comments,
    string? Copyright,
    string[] Documenters,
    string? License,
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
    
    // Window parameters
    string Title,
    int Width,
    int Height
);
