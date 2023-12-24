using System;
using System.Threading.Tasks;
using Silk.NET.Windowing;

namespace Tomat.GameBreaker.Windowing;

// Loosely based on https://github.com/GovermanGambo/Akithos/tree/main.

/// <summary>
///     The abstract application shell of a program.
/// </summary>
public abstract class Application : IDisposable {
    protected delegate T WindowFactory<out T>(ref WindowOptions windowOptions) where T : ImGuiWindow;

    private int windowCount;

    protected virtual T InitializeWindow<T>(WindowOptions windowOptions, WindowFactory<T> windowFactory) where T : ImGuiWindow {
        var windowInstance = windowFactory(ref windowOptions);
        var window = Window.Create(windowOptions);
        windowInstance.Initialize(window);
        Task.Run(
            () => {
                windowInstance.Run();
                windowInstance.Dispose();
            }
        );

        windowCount++;
        window.Closing += () => windowCount--;
        return windowInstance;
    }

    public void WaitForWindows() {
        while (windowCount > 0) { }
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
