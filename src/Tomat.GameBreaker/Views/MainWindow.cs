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

        var actionAbout = Gio.SimpleAction.New("about", null);
        actionAbout.OnActivate += AboutOnActivate;
        AddAction(actionAbout);
        this.application.SetAccelsForAction("win.about", new[] { "F1" });

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
        var attributes = Pango.FontDescription;
        projectsHeader.SetAttributes();
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
