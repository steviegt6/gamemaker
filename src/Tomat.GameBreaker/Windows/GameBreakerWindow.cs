using System;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Windowing;

namespace Tomat.GameBreaker.Windows;

internal abstract class GameBreakerWindow : ImGuiWindow {
    public new GameBreakerApplication App {
        get => (GameBreakerApplication) base.App;
        set => base.App = value;
    }

    protected GameBreakerWindow(Application app, ref WindowOptions options) : base(app, ref options) {
        if (app is not GameBreakerApplication)
            throw new ArgumentException("Application must be of type GameBreakerApplication.", nameof(app));
    }
}
