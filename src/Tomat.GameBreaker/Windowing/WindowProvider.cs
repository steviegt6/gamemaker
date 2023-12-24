using Tomat.GameBreaker.Logging;

namespace Tomat.GameBreaker.Windowing;

public interface IWindowProvider {
    bool Initialize();

    T CreateWindow<T>(T window) where T : ImGuiWindow;
}

internal sealed class Sdl2WindowProvider : IWindowProvider {
    private readonly Logger logger = Log.GetLogger(typeof(Sdl2WindowProvider));

    public bool Initialize() {
        logger.Debug("Initializing SDL...");
        if (SDL_Init(SDL_INIT_EVERYTHING) == 0)
            return true;

        logger.Error($"Failed to initialize SDL: {SDL_GetError()}");
        return false;
    }

    public T CreateWindow<T>(T window) where T : ImGuiWindow {
        logger.Debug($"Creating SDL window \"{window.WindowName}\"...");
        return window;
    }
}
