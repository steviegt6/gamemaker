namespace Tomat.GameBreaker.Windowing;

public abstract class ImGuiWindow {
    public string WindowName { get; }

    protected ImGuiWindow(string windowName) {
        WindowName = windowName;
    }
}
