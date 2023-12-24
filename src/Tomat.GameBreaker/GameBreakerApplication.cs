using Silk.NET.Windowing;
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
}
