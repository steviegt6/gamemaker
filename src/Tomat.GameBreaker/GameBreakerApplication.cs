using Silk.NET.Maths;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Features.Splash;
using Tomat.GameBreaker.Windowing;

namespace Tomat.GameBreaker;

internal sealed class GameBreakerApplication : Application {
    public GameBreakerApplication() {
        InitializeWindow<SplashWindow>(
            WindowOptions.Default with {
                Size = new Vector2D<int>(640, 400),
                Title = "Splash",
                WindowBorder = WindowBorder.Hidden,
            },
            window => new SplashWindow(window)
        );
    }
}
