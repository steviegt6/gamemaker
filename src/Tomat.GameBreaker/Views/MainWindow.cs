using Tomat.GameBreaker.Controls;

namespace Tomat.GameBreaker.Views;

public class MainWindow : Gtk.ApplicationWindow {
    private readonly MainWindowController controller;
    private readonly Adw.Application application;

    public MainWindow(MainWindowController controller, Adw.Application application) {
        this.controller = controller;
        this.application = application;

        //SetIconName(this.controller.ApplicationId);
        SetTitle(this.controller.WindowTitle);
        SetDefaultSize(this.controller.DefaultWidth, this.controller.DefaultHeight);

        var aboutAction = Gio.SimpleAction.New("about", null);
        aboutAction.OnActivate += AboutOnActivate;
        AddAction(aboutAction);
        this.application.SetAccelsForAction("win.about", new[] { "F1" });

        // Header bar
        var headerBar = Adw.HeaderBar.New();
        headerBar.TitleWidget = Adw.WindowTitle.New(this.controller.WindowTitle, "");

        var menuButton = Gtk.MenuButton.New();
        menuButton.IconName = "view-more-symbolic";
        var menuPopover = Gtk.PopoverMenu.NewFromModel(null);
        //var menuBox = Gtk.Box.New(Gtk.Orientation.Vertical, 0);
        //var aboutMenuItem = Gtk.Button.NewWithLabel("About");
        //aboutMenuItem.OnClicked += (_, _) => actionAbout.Activate(null);
        //menuBox.Append(aboutMenuItem);
        //menuPopover.Child = (menuBox);
        var aboutShortcutController = Gtk.ShortcutController.New();
        var aboutShortcut = Gtk.Shortcut.New(Gtk.ShortcutTrigger.ParseString("F1"), Gtk.ShortcutAction.ParseString("win.about"));
        aboutShortcutController.AddShortcut(aboutShortcut);
        //menuPopover.AddChild(aboutShortcutController, "about");
        menuButton.Popover = menuPopover;
        headerBar.PackEnd(menuButton);

        SetTitlebar(headerBar);

        // Window contents
        var box = Gtk.Box.New(Gtk.Orientation.Vertical, 0);

        var projectsHeader = Gtk.Label.New("Projects");
        projectsHeader.SetHalign(Gtk.Align.Start);
        projectsHeader.SetValign(Gtk.Align.Start);
        projectsHeader.SetHexpand(true);
        projectsHeader.SetVexpand(false);
        projectsHeader.SetMarginStart(10);
        projectsHeader.SetMarginEnd(10);
        projectsHeader.SetMarginTop(10);
        projectsHeader.SetMarginBottom(10);
        box.Append(projectsHeader);

        SetChild(box);
    }

    public void Start() {
        application.AddWindow(this);

        Show();
    }

    private void AboutOnActivate(Gio.SimpleAction sender, Gio.SimpleAction.ActivateSignalArgs args) {
        var dialog = Adw.AboutWindow.New();
        dialog.ApplicationIcon = controller.ApplicationIcon;
        dialog.ApplicationName = controller.ApplicationName;
        dialog.Artists = controller.Artists;
        dialog.Comments = controller.Comments;
        dialog.Copyright = controller.Copyright;
        dialog.DebugInfo = controller.DebugInfo;
        dialog.DebugInfoFilename = controller.DebugInfoFilename;
        dialog.Designers = controller.Designers;
        dialog.DeveloperName = controller.DeveloperName;
        dialog.Developers = controller.Developers;
        dialog.Documenters = controller.Documenters;
        dialog.IssueUrl = controller.IssueUrl;
        dialog.License = controller.License;
        dialog.LicenseType = controller.LicenseType;
        dialog.ReleaseNotes = controller.ReleaseNotes;
        dialog.ReleaseNotesVersion = controller.ReleaseNotesVersion;
        dialog.SupportUrl = controller.SupportUrl;
        dialog.TranslatorCredits = controller.TranslatorCredits;
        dialog.Version = controller.Version;
        dialog.Website = controller.Website;
        dialog.Present();
    }
}
