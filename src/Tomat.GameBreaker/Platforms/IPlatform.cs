using Tomat.GameBreaker.Windowing;

namespace Tomat.GameBreaker.Platforms;

internal interface IPlatform {
    void MakeWindowTransparent(ImGuiWindow window);
}
