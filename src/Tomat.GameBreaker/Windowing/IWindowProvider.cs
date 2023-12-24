using Tomat.GameBreaker.Logging;

namespace Tomat.GameBreaker.Windowing;

public interface IWindowProvider {
    bool Initialize(Logger logger);

    void Terminate(Logger logger);

    T? CreateWindow<T>(Logger logger, T window) where T : ImGuiWindow;
}
