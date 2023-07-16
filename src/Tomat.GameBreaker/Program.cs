using System;
using System.Globalization;
using System.Reflection;
using Tomat.GameBreaker.Controls;
using Tomat.GameBreaker.Util;
using Tomat.GameBreaker.Views;

namespace Tomat.GameBreaker;

internal static class Program {
    public static readonly Assembly ASSEMBLY = typeof(Program).Assembly;

    private const string application_id = "dev.tomat.gamebreaker";

    [STAThread]
    internal static int Main() {
        if (CultureInfo.CurrentCulture.Equals(CultureInfo.InvariantCulture))
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

        var application = Adw.Application.New(application_id, Gio.ApplicationFlags.FlagsNone);
        var version = ASSEMBLY.GetName().Version ?? new Version(0, 0, 0, 0);
        application.OnActivate += (sender, _) => {
            if (sender is not Adw.Application app)
                throw new InvalidOperationException("OnActivate sender is not an Adw.Application.");

            var mainWindowController = new MainWindowController(
                ApplicationId: application_id,

                // Adw.AboutWindow options.
                ApplicationIcon: "", //application_id,
                ApplicationName: "Tomat.GameBreaker",
                Artists: Array.Empty<string>(),
                Comments: "Tomat.GameBreaker is a tool for reverse-engineering and modifying in-production GameMaker games.",
                Copyright: "Copyright © 2023 Tomat and Tomat.GameBreaker contributors 2023\n"
                         + "Copyright © 2023 colinator27 and DogScepter contributors\n"
                         + "Copyright © 2023 krzys-h and UndertaleModTool contributors",
                DebugInfo: "",
                DebugInfoFilename: "debug_info.txt",
                Designers: new [] { "Tomat" },
                Documenters: new[] { "Tomat" },
                DeveloperName: "Tomat",
                Developers: new[] { "Tomat" } ,
                IssueUrl: "https://github.com/steviegt6/gamemaker/issues/",
                License: "GNU General Public License, version 3",
                LicenseType: Gtk.License.Gpl30,
                ReleaseNotes: ReleaseNotes.GetReleaseNotes(),
                ReleaseNotesVersion: ReleaseNotes.RELEASE_NOTES_VERSION,
                SupportUrl: "https://discord.gg/KvqKGQNbhr",
                TranslatorCredits: "",
                Version: version.ToString(),
                Website: "https://github.com/steviegt6/gamemaker",

                // Window display options.
                WindowTitle: $"Tomat.GameBreaker - {version}",
                DefaultWidth: 800,
                DefaultHeight: 600
            );

            var window = new MainWindow(mainWindowController, app);
            window.Start();
        };
        return application.Run();
    }
}
