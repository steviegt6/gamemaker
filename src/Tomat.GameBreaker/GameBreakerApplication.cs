using System;
using Silk.NET.Core;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Tomat.GameBreaker.Features.Splash;
using Tomat.GameBreaker.Framework.Logging;
using Tomat.GameBreaker.Framework.Windowing;

namespace Tomat.GameBreaker;

internal sealed class GameBreakerApplication : Application {
    private readonly Logger logger = Log.AsType<GameBreakerApplication>();

    public GameBreakerApplication() {
        logger.Debug("Initializing application...");

        logger.Debug("Initializing splash window...");
        InitializeWindow(
            WindowOptions.Default,
            (ref WindowOptions options) => new SplashWindow(ref options)
        );
    }

    protected override unsafe T InitializeWindow<T>(ref WindowOptions windowOptions, WindowFactory<T> windowFactory) {
        var oldName = windowOptions.Title;
        var window = base.InitializeWindow(ref windowOptions, windowFactory);
        logger.Debug($"Received window initialization request (\"{oldName}\" -> \"{windowOptions.Title}\")");

        window.Window.Load += () => {
            using var stream = typeof(GameBreakerApplication).Assembly.GetManifestResourceStream("Tomat.GameBreaker.resources.icon.png")!;
            using var image = Image.Load<Rgba32>(stream);
            var bytes = new byte[image.Width * image.Height * sizeof(Rgba32)];
            image.CopyPixelDataTo(bytes);
            var rawImg = new RawImage(image.Width, image.Height, new Memory<byte>(bytes));
            window.Window.SetWindowIcon(ref rawImg);
        };

        return window;
    }
}
