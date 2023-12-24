using Silk.NET.Maths;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Windowing;

namespace Tomat.GameBreaker.Features.Splash;

internal sealed class SplashWindow : ImGuiWindow {
    public SplashWindow(ref WindowOptions options) : base(ref options) {
        options = options with {
            Size = new Vector2D<int>(640, 400),
            Title = "Splash",
            WindowBorder = WindowBorder.Hidden,
        };
    }
}
