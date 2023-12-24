using Silk.NET.Windowing;
using Tomat.GameBreaker.Platforms;
using Tomat.GameBreaker.Windowing;

namespace Tomat.GameBreaker.Features.Splash;

internal sealed class SplashWindow : ImGuiWindow {
    public SplashWindow(IWindow window) : base(window) { }

    public override void Initialize() {
        base.Initialize();

        /*Window.Resizable = false;
        Window.BorderVisible = false;
        Window.X = Sdl2Native.SDL_WINDOWPOS_CENTERED;
        Window.Y = Sdl2Native.SDL_WINDOWPOS_CENTERED;*/
        Platform.MakeWindowTransparent(this);
    }
}
