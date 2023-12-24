using System;
using Veldrid;
using Veldrid.Sdl2;

namespace Tomat.GameBreaker.Windowing;

/// <summary>
///     An ImGui window, to be used with <see cref="Application"/>.
///     <br />
///     Contains necessities for rendering ImGui to a Veldrid-provided SDL
///     window.
/// </summary>
public abstract class ImGuiWindow : IDisposable {
    /// <summary>
    ///     The underlying SDL window.
    /// </summary>
    public Sdl2Window Window { get; }

    /// <summary>
    ///     The graphics device to which this window belongs.
    /// </summary>
    public GraphicsDevice GraphicsDevice { get; }

    /// <summary>
    ///     The command list.
    /// </summary>
    /// <remarks>
    ///     May be null before initialization and after disposal.
    /// </remarks>
    public CommandList? CommandList { get; set; }

    public event Action? OnResized;

    protected ImGuiWindow(Sdl2Window window, GraphicsDevice graphicsDevice) {
        Window = window;
        GraphicsDevice = graphicsDevice;

        // TODO: I actually don't know if we can just do
        // Window.Resized += OnResized or if it won't respond to later
        // subscriptions. One would assume so, but better safe than sorry.
        Window.Resized += () => {
            OnResized?.Invoke();
        };
    }

    /// <summary>
    ///     Initializes this window for running through a
    ///     <see cref="WindowRunContext"/> in an <see cref="Application"/>.
    /// </summary>
    public virtual void Initialize() {
        CommandList = GraphicsDevice.ResourceFactory.CreateCommandList();
    }

    /// <summary>
    ///     Updates this window through a <see cref="WindowRunContext"/>
    ///     in an <see cref="Application"/>.
    /// </summary>
    public virtual void Update() { }

    protected virtual void Dispose(bool disposing) {
        if (!disposing)
            return;

        GraphicsDevice.Dispose();
        CommandList?.Dispose();
        Window.Close();
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
