using System;
using System.Collections.Generic;
using Silk.NET.Windowing;

namespace Tomat.GameBreaker.Framework.Windowing;

// Loosely based on https://github.com/GovermanGambo/Akithos/tree/main.

/// <summary>
///     The abstract application shell of a program.
/// </summary>
public abstract class Application : IDisposable {
    protected delegate T WindowFactory<out T>(Application app, ref WindowOptions windowOptions) where T : ImGuiWindow;

    protected List<ImGuiWindow> Windows { get; } = new();

    private readonly List<ImGuiWindow> queuedWindows = new();

    private int windowCount;

    protected T InitializeWindow<T>(WindowOptions windowOptions, WindowFactory<T> windowFactory) where T : ImGuiWindow {
        return InitializeWindow(ref windowOptions, windowFactory);
    }

    protected virtual T InitializeWindow<T>(ref WindowOptions windowOptions, WindowFactory<T> windowFactory) where T : ImGuiWindow {
        var windowInstance = windowFactory(this, ref windowOptions);
        var window = Window.Create(windowOptions);
        windowInstance.Initialize(window);

        windowCount++;
        window.Closing += () => {
            windowCount--;
            window.Dispose();
        };

        queuedWindows.Add(windowInstance);
        return windowInstance;
    }

    public virtual void UpdateWindows() {
        foreach (var window in queuedWindows) {
            window.Window.Initialize();
            Windows.Add(window);
        }

        queuedWindows.Clear();

        foreach (var window in Windows)
            window.DoEvents();

        Windows.RemoveAll(window => window.Window.IsClosing);
    }

    public virtual bool ShouldExit() {
        // By default, we'll just exit when no windows exist.
        return windowCount <= 0;
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
