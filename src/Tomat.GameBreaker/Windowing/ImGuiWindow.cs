using System;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Tomat.GameBreaker.Windowing;

/// <summary>
///     An ImGui window, to be used with <see cref="Application"/>.
///     <br />
///     Contains necessities for rendering ImGui to a window.
/// </summary>
public abstract class ImGuiWindow : IDisposable {
    /// <summary>
    ///     The underlying window.
    /// </summary>
    public IWindow Window { get; }

    public ImGuiController Controller { get; private set; } = null!;

    public GL Gl { get; private set; } = null!;

    public IInputContext InputContext { get; private set; } = null!;

    protected ImGuiWindow(IWindow window) {
        Window = window;

        Window.Load += () => {
            Controller = new ImGuiController(
                Gl = window.CreateOpenGL(),
                window,
                InputContext = window.CreateInput()
            );

            Initialize();
        };

        Window.FramebufferResize += s => {
            Gl.Viewport(s);
        };

        Window.Render += delta => {
            Controller.Update((float) delta);

            Gl.ClearColor(0, 0, 0, 0);
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            Update();

            Controller.Render();
        };

        Window.Closing += () => {
            Controller.Dispose();
            InputContext.Dispose();
            Gl.Dispose();
        };
    }

    public virtual void Run() {
        Window.Run();
    }

    public virtual void Initialize() { }

    public virtual void Update() { }

    protected virtual void Dispose(bool disposing) {
        if (disposing)
            Window.Dispose();
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
