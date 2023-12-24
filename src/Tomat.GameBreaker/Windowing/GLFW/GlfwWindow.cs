using Silk.NET.GLFW;

namespace Tomat.GameBreaker.Windowing.GLFW;

internal sealed unsafe class GlfwWindow : IWindow {
    private readonly WindowHandle* window;
    private readonly Glfw glfw;

    // GLFW provides no API function for retrieving the window title, so we have
    // to keep track of it ourselves.
    private string title = string.Empty;

    public string Title {
        get => title;
        set => glfw.SetWindowTitle(window, title = value);
    }

    public int Width {
        get {
            glfw.GetWindowSize(window, out var width, out _);
            return width;
        }

        set => glfw.SetWindowSize(window, value, Height);
    }

    public int Height {
        get {
            glfw.GetWindowSize(window, out _, out var height);
            return height;
        }

        set => glfw.SetWindowSize(window, Width, value);
    }

    public int X {
        get {
            glfw.GetWindowPos(window, out var x, out _);
            return x;
        }

        set => glfw.SetWindowPos(window, value, Y);
    }

    public int Y {
        get {
            glfw.GetWindowPos(window, out _, out var y);
            return y;
        }

        set => glfw.SetWindowPos(window, X, value);
    }

    public GlfwWindow(WindowHandle* window, Glfw glfw) {
        this.window = window;
        this.glfw = glfw;
    }

    public void Dispose() {
        glfw.DestroyWindow(window);
    }
}
