using System;
using System.Globalization;
using Tomat.GameBreaker.Controls;
using Tomat.GameBreaker.Views;

namespace Tomat.GameBreaker;

internal static class Program {
    [STAThread]
    internal static int Main() {
        if (CultureInfo.CurrentCulture.Equals(CultureInfo.InvariantCulture))
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

        var application = Gtk.Application.New("dev.tomat.gamebreaker", Gio.ApplicationFlags.DefaultFlags);
        var version = typeof(Program).Assembly.GetName().Version ?? new Version(0, 0, 0, 0);
        application.OnActivate += (sender, _) => {
            if (sender is not Gtk.Application app)
                throw new InvalidOperationException("OnActivate sender is not a Gtk.Application.");

            var mainWindowController = new MainWindowController(
                // AboutDialog options.
                Artists: Array.Empty<string>(),
                Authors: new[] { "Tomat" },
                Comments: "Tomat.GameBreaker is a tool for reverse-engineering and modifying in-production GameMaker games.",
                Copyright: "Copyright © 2023 Tomat and Tomat.GameBreaker contributors 2023\n"
                         + "Copyright © 2023 colinator27 and DogScepter contributors\n"
                         + "Copyright © 2023 krzys-h and UndertaleModTool contributors",
                Documenters: new[] { "Tomat" },
                License: "GNU General Public License, version 3",
                LicenseType: Gtk.License.Gpl30,
                Logo: null!,
                LogoIconName: null,
                ProgramName: "Tomat.GameBreaker",
                SystemInformation: null,
                TranslatorCredits: null,
                Version: version.ToString(),
                Website: "https://github.com/steviegt6/gamemaker",
                WebsiteLabel: null,
                WrapLicense: true,

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
