using System;
using Gio;

namespace Tomat.GameBreaker;

internal static class Program {
    [STAThread]
    internal static int Main() {
        var application = Gtk.Application.New("dev.tomat.gamebreaker", ApplicationFlags.DefaultFlags);
        application.OnActivate += (sender, _) => {
            var window = Gtk.ApplicationWindow.New((Gtk.Application) sender);
            window.Title = "GameBreaker";
            window.SetDefaultSize(800, 600);
            window.Show();
        };
        return application.Run();
    }
}
