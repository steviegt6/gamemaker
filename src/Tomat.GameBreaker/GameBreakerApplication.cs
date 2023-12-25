using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Logging;
using Tomat.GameBreaker.Framework.Windowing;
using Tomat.GameBreaker.Utilities;
using Tomat.GameBreaker.Windows.Main;
using Tomat.GameBreaker.Windows.Splash;

namespace Tomat.GameBreaker;

internal sealed class GameBreakerApplication : Application {
    private readonly Logger logger = Log.AsType<GameBreakerApplication>();

    public GameBreakerSettings Settings { get; }

    // We'll use splash to queue start-up tasks and also keep the application
    // alive (window will remain open but hidden).
    public SplashWindow Splash { get; }

    public MainWindow? Main { get; private set; }

    public GameBreakerApplication() {
        logger.Debug("Initializing application...");

        logger.Debug("Reading settings file...");
        if (File.Exists("gamebreaker.settings.json"))
            Settings = JsonConvert.DeserializeObject<GameBreakerSettings>(File.ReadAllText("gamebreaker.settings.json"))!;

        if (Settings is null) {
            logger.Debug("Settings file not found, creating new settings file...");
            Settings = new GameBreakerSettings();
        }

        logger.Debug("Initializing splash window...");
        Splash = InitializeWindow(
            WindowOptions.Default,
            (Application app, ref WindowOptions options) => new SplashWindow(app, ref options)
        );

        Splash.Window.Load += () => {
            Splash.RunQueuedTasks();
        };

        Splash.QueueTask(
            "Test 1",
            1f,
            async t => {
                while (t.Progress < 1f) {
                    t.Progress += 0.01f;
                    await Task.Delay(20);
                }
            }
        );

        Splash.QueueTask(
            "Test 2",
            2f,
            async t => {
                while (t.Progress < 1f) {
                    t.Progress += 0.05f;
                    await Task.Delay(30);
                }
            }
        );

        // Once all start-up procedures are complete, we can hide the splash and
        // open the main window.
        Splash.OnCompleted += () => {
            // Splash.Window.IsVisible = false;

            // Also make it open the main window.
            Main = InitializeWindow(
                WindowOptions.Default,
                (Application app, ref WindowOptions options) => new MainWindow(app, ref options)
            );

            Main.Window.Closing += () => {
                Splash.Window.Close();
                Splash.Dispose();
            };
        };
    }

    protected override T InitializeWindow<T>(ref WindowOptions windowOptions, WindowFactory<T> windowFactory) {
        var oldName = windowOptions.Title;
        var window = base.InitializeWindow(ref windowOptions, windowFactory);
        logger.Debug($"Received window initialization request (\"{oldName}\" -> \"{windowOptions.Title}\")");

        window.Window.Load += () => {
            ImageExt.FromAssemblyResource("resources.icon.png", out var img);
            var rawImg = img.AsRaw();
            window.Window.SetWindowIcon(ref rawImg);
        };

        return window;
    }

    public override bool ShouldExit() {
        var res = base.ShouldExit();

        if (!res)
            return res;

        logger.Debug("All windows closed, exiting...");
        File.WriteAllText("gamebreaker.settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));
        return true;
    }
}
