using Tomat.GameBreaker.Controls;

namespace Tomat.GameBreaker.Views;

public class MainWindow : Gtk.ApplicationWindow {
    private readonly MainWindowController controller;
    private readonly Gtk.Application application;

    public MainWindow(MainWindowController controller, Gtk.Application application) {
        this.controller = controller;
        this.application = application;

        SetTitle(this.controller.WindowTitle);
        SetDefaultSize(this.controller.DefaultWidth, this.controller.DefaultHeight);
    }

    public void Start() {
        application.AddWindow(this);

        var menu = Gio.Menu.New();
        menu.AppendItem(Gio.MenuItem.New("test", "action"));
        application.SetMenubar(menu);

        Show();

        var dialog = Gtk.AboutDialog.New();
        dialog.Authors = controller.Authors;
        dialog.Comments = controller.Comments;
        dialog.Copyright = controller.Copyright;
        dialog.License = controller.License;
        dialog.Version = controller.Version;
        dialog.Website = controller.Website;
        dialog.LicenseType = controller.LicenseType;
        dialog.ProgramName = controller.ProgramName;
        dialog.Present();
    }
}
