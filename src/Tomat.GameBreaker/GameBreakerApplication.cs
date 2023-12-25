using System.Threading.Tasks;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Logging;
using Tomat.GameBreaker.Framework.Windowing;
using Tomat.GameBreaker.Utilities;
using Tomat.GameBreaker.Windows.Splash;

namespace Tomat.GameBreaker;

internal sealed class GameBreakerApplication : Application {
    private readonly Logger logger = Log.AsType<GameBreakerApplication>();

    public GameBreakerApplication() {
        logger.Debug("Initializing application...");

        logger.Debug("Initializing splash window...");
        var splash = InitializeWindow(
            WindowOptions.Default,
            (ref WindowOptions options) => new SplashWindow(ref options)
        );

        splash.StartTask(
            "Test 1",
            1f,
            async t => {
                while (t.Progress < 1f) {
                    t.Progress += 0.01f;
                    await Task.Delay(20);
                }
            }
        );

        splash.StartTask(
            "Test 2",
            2f,
            async t => {
                while (t.Progress < 1f) {
                    t.Progress += 0.05f;
                    await Task.Delay(30);
                }
            }
        );
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
}
