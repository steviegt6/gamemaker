using System;
using System.Collections.Generic;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Tomat.GameBreaker.Windowing;

// Loosely based on https://github.com/GovermanGambo/Akithos/tree/main.

/// <summary>
///     The abstract application shell of a program.
/// </summary>
public abstract class Application : IDisposable {
    protected List<WindowRunContext> Windows { get; } = new();

    private readonly List<ImGuiWindow> windowsToInitialize = new();

    public virtual T InitializeWindow<T>(WindowCreateInfo windowCreateInfo, Func<Sdl2Window, GraphicsDevice, T> windowFactory) where T : ImGuiWindow {
        var window = VeldridStartup.CreateWindow(windowCreateInfo);
        var graphicsDevice = VeldridStartup.CreateGraphicsDevice(
            window,
            new GraphicsDeviceOptions {
                PreferDepthRangeZeroToOne = true,
                PreferStandardClipSpaceYDirection = true,
                ResourceBindingModel = ResourceBindingModel.Improved,
                SyncToVerticalBlank = true,
            }
        );

        var windowInstance = windowFactory(window, graphicsDevice);
        windowsToInitialize.Add(windowInstance);
        return windowInstance;
    }

    /// <summary>
    ///     Runs the application.
    /// </summary>
    public virtual void Run() {
        foreach (var window in windowsToInitialize)
            Windows.Add(new WindowRunContext(window));

        windowsToInitialize.Clear();

        foreach (var window in Windows)
            window.Run();

        Windows.RemoveAll(x => x.Disposed);
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
