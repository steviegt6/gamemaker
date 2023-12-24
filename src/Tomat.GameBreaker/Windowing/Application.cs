using System;
using System.Collections.Generic;
using Silk.NET.Windowing;

namespace Tomat.GameBreaker.Windowing;

// Loosely based on https://github.com/GovermanGambo/Akithos/tree/main.

/// <summary>
///     The abstract application shell of a program.
/// </summary>
public abstract class Application : IDisposable {
    protected List<ImGuiWindow> Windows { get; } = new();

    protected virtual T InitializeWindow<T>(WindowOptions windowOptions, Func<IWindow, T> windowFactory) where T : ImGuiWindow {
        var window = Window.Create(windowOptions);
        var windowInstance = windowFactory(window);
        windowInstance.Initialize();
        windowInstance.Run();
        return windowInstance;
    }

    /// <summary>
    ///     Disposes of this application.
    /// </summary>
    /// <param name="disposing">
    ///     Whether to dispose of managed resources.
    /// </param>
    protected virtual void Dispose(bool disposing) { }

    /// <summary>
    ///     Disposes of this application.
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
