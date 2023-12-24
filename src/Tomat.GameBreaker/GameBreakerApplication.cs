using Tomat.GameBreaker.Features.Splash;
using Tomat.GameBreaker.Windowing;
using Veldrid.StartupUtilities;

namespace Tomat.GameBreaker;

internal sealed class GameBreakerApplication : Application {
    public GameBreakerApplication() {
        InitializeWindow<SplashWindow>(
            new WindowCreateInfo {
                WindowWidth = 640,
                WindowHeight = 400,
                WindowTitle = "Splash",
            },
            (window, device) => new SplashWindow(window, device)
        );
    }
}
