using System;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Tomat.GameBreaker.Framework.Windowing;

/// <summary>
///     An ImGui window, to be used with <see cref="Application"/>.
///     <br />
///     Contains necessities for rendering ImGui to a window.
/// </summary>
public abstract class ImGuiWindow : IDisposable {
    private static readonly object init_lock = new();

    /// <summary>
    ///     The underlying window.
    /// </summary>
    public IWindow Window { get; private set; } = null!;

    public ImGuiController Controller { get; private set; } = null!;

    public GL Gl { get; private set; } = null!;

    public IInputContext InputContext { get; private set; } = null!;

    protected Application App { get; set; }

    protected ImGuiWindow(Application app, ref WindowOptions options) {
        App = app;
    }

    public virtual void DoEvents() {
        Window.DoEvents();

        if (!Window.IsClosing)
            Window.DoUpdate();

        if (Window.IsClosing)
            return;

        Window.DoRender();
    }

    public virtual void Initialize(IWindow window) {
        Window = window;

        Window.Load += () => {
            lock (init_lock) {
                Controller = new ImGuiController(
                    Gl = window.CreateOpenGL(),
                    window,
                    InputContext = window.CreateInput()
                );
            }
        };

        Window.FramebufferResize += s => {
            Gl.Viewport(s);
        };

        window.Update += Update;

        Window.Render += delta => {
            Controller.Update((float) delta);

            Gl.ClearColor(0, 0, 0, 0);
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            Render(delta);

            Controller.Render();
        };

        Window.Closing += () => {
            Controller.Dispose();
            InputContext.Dispose();
            Gl.Dispose();
        };

        window.Initialize();
    }

    protected virtual void Update(double delta) { }

    protected virtual void Render(double delta) { }

    protected virtual void Dispose(bool disposing) {
        if (disposing)
            Window.Dispose();
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
