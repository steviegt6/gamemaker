using Silk.NET.Maths;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Windowing;

namespace Tomat.GameBreaker.Features.Splash;

internal sealed class SplashWindow : ImGuiWindow {
    public SplashWindow(ref WindowOptions options) : base(ref options) {
        options = options with {
            Size = new Vector2D<int>(640, 400),
            Title = "Splash",
            WindowBorder = WindowBorder.Hidden,
            IsVisible = false,
            TransparentFramebuffer = true,
        };
    }

    public override void Initialize(IWindow window) {
        base.Initialize(window);

        Window.Load += () => {
            Window.Center();
            Window.IsVisible = true;
        };
    }

    public override void Render(double delta) {
        base.Render(delta);

        ImGuiNET.ImGui.ShowDemoWindow();
    }
}
