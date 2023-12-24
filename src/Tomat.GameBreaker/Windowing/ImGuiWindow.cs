using System;
using Tomat.GameBreaker.Logging;

namespace Tomat.GameBreaker.Windowing;

public abstract class ImGuiWindow : IDisposable {
    /// <summary>
    ///     Internal name for window identification.
    /// </summary>
    public string InternalName { get; }

    /// <summary>
    ///     The title of the window upon initialization.
    /// </summary>
    public string InitialTitle { get; }

    /// <summary>
    ///     The width of the window upon initialization.
    /// </summary>
    public int InitialWidth { get; }

    /// <summary>
    ///     The height of the window upon initialization.
    /// </summary>
    public int InitialHeight { get; }

    /// <summary>
    ///     The underlying, implementation-specific window.
    /// </summary>
    /// <remarks>
    ///     Provides some guaranteed functionality.
    ///     <br />
    ///     May be <see langword="null"/> if the window has been instantiated
    ///     but not yet created through
    ///     <see cref="IWindowProvider.CreateWindow{T}"/>.
    /// </remarks>
    public IWindow Window { get; set; } = null!;

    protected ImGuiWindow(string internalName, string initialTitle, int initialWidth, int initialHeight) {
        InternalName = internalName;
        InitialTitle = initialTitle;
        InitialWidth = initialWidth;
        InitialHeight = initialHeight;
    }

    public virtual void OnRun(Logger logger) { }

    public virtual void OnUpdate(Logger logger) { }

    protected virtual void Dispose(bool disposing) {
        if (disposing)
            Window.Dispose();
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
