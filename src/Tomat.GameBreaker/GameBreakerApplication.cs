using System;
using Silk.NET.Core;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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
            t => {
                t.Progress = 1f;
            }
        );
        splash.StartTask(
            "Test 2",
            2f,
            t => {
                t.Progress = 0.5f;
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
