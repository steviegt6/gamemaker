using System.IO;
using Tomat.Houli.Launcher.Controls;

namespace Tomat.Houli.Launcher.Views;

public sealed class MainWindow : Gtk.ApplicationWindow {
    private readonly MainWindowController controller;
    private readonly Gtk.Application application;

    public MainWindow(MainWindowController controller, Gtk.Application application) {
        this.controller = controller;
        this.application = application;

        // add f1 command to show about dialog
        var aboutAction = Gio.SimpleAction.New("about", null);
        aboutAction.OnActivate += (_, _) => CreateAboutDialog().Show();
        AddAction(aboutAction);
        application.AddAction(aboutAction);
        application.SetAccelsForAction("win.about", new[] { "F1" });
        /*var iconTheme = Gtk.IconTheme.GetForDisplay(Display!);
        iconTheme.LookupIcon();*/
        var iconTheme = Gtk.IconTheme.GetForDisplay(Display!);
        iconTheme.AddResourcePath(Directory.GetCurrentDirectory());

        SetIconName(this.controller.LogoIconName);
        SetTitle(this.controller.Title);
        SetDefaultSize(this.controller.Width, this.controller.Height);

        Adw.Module.Initialize();
        var headerBar = Adw.HeaderBar.New();
        SetTitlebar(headerBar);
        var menuBar = Gio.Menu.New();
        menuBar.AppendSubmenu("_File", Gio.Menu.New());
        headerBar.PackEnd(new Gtk.MenuButton {
            MenuModel = menuBar,
            IconName = "open-menu-symbolic",
        });
    }

    public void Start() {
        application.AddWindow(this);
        Show();
    }

    private Gtk.AboutDialog CreateAboutDialog() {
        var dialog = new Gtk.AboutDialog();
        dialog.Authors = controller.Authors;
        dialog.Comments = controller.Comments;
        dialog.License = controller.License;
        dialog.LicenseType = controller.LicenseType;
        dialog.Logo = controller.Logo;
        dialog.LogoIconName = controller.LogoIconName;
        dialog.ProgramName = controller.ProgramName;
        dialog.TranslatorCredits = controller.TranslatorCredits;
        dialog.Version = controller.Version;
        dialog.Website = controller.Website;
        dialog.WebsiteLabel = controller.WebsiteLabel;
        return dialog;
    }
}
