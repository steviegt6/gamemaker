using System;
using System.Globalization;
using System.Reflection;
using Tomat.Houli.Launcher.Controls;
using Tomat.Houli.Launcher.Views;

namespace Tomat.Houli.Launcher;

internal static class Program {
    private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
    private const string application_id = "dev.tomat.houli";

    internal static int Main(string[] args) {
        if (CultureInfo.CurrentCulture.Equals(CultureInfo.InvariantCulture))
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

        var application = Gtk.Application.New(application_id, Gio.ApplicationFlags.FlagsNone);
        application.OnActivate += (sender, _) => {
            var controller = new MainWindowController(
                // AboutDialog
                Artists: Array.Empty<string>(),
                Authors: new[] { "Tomat" },
                Comments: "Tomat.Houli - GameMaker mod launcher.",
                Copyright: "Copyright (c) 2023 Tomat",
                Documenters: new[] { "Tomat" },
                License: "GNU General Public License, version 3",
                LicenseType: Gtk.License.Gpl30,
                Logo: default!,
                LogoIconName: application_id,
                ProgramName: "Tomat.Houli.Launcher",
                SystemInformation: null,
                TranslatorCredits: null,
                Version: (assembly.GetName().Version ?? new Version(0, 0)).ToString(),
                Website: "https://github.com/steviegt6/gamemaker",
                WebsiteLabel: "GitHub",
                WrapLicense: false,
                
                // Window parameters
                Title: "Tomat.Houli.Launcher",
                Width: 800,
                Height: 600
            );
            var window = new MainWindow(controller, (Gtk.Application) sender);
            window.Start();
        };
        return application.Run();
    }
}
