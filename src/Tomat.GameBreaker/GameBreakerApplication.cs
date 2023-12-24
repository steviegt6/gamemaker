using System;
using Silk.NET.Core;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Tomat.GameBreaker.Features.Splash;
using Tomat.GameBreaker.Windowing;

namespace Tomat.GameBreaker;

internal sealed class GameBreakerApplication : Application {
    public GameBreakerApplication() {
        InitializeWindow(
            WindowOptions.Default,
            (ref WindowOptions options) => new SplashWindow(ref options)
        );
    }

    protected override unsafe T InitializeWindow<T>(WindowOptions windowOptions, WindowFactory<T> windowFactory) {
        var window = base.InitializeWindow(windowOptions, windowFactory);

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
