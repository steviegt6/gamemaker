using Tomat.GameBreaker.Platforms;
using Tomat.GameBreaker.Windowing;
using Veldrid;
using Veldrid.Sdl2;

namespace Tomat.GameBreaker.Features.Splash;

internal sealed class SplashWindow : ImGuiWindow {
    public SplashWindow(Sdl2Window window, GraphicsDevice graphicsDevice) : base(window, graphicsDevice) { }

    public override void Initialize() {
        base.Initialize();

        Window.Resizable = false;
        Window.BorderVisible = false;
        Window.X = Sdl2Native.SDL_WINDOWPOS_CENTERED;
        Window.Y = Sdl2Native.SDL_WINDOWPOS_CENTERED;
        Platform.MakeWindowTransparent(this);
    }
}
