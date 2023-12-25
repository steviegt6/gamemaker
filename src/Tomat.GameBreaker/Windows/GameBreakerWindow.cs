using System;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Windowing;

namespace Tomat.GameBreaker.Windows;

internal abstract class GameBreakerWindow : ImGuiWindow {
    public new GameBreakerApplication App {
        get => (GameBreakerApplication) base.App;
        set => base.App = value;
    }

    protected GameBreakerWindow(ref WindowOptions options) : base(ref options) { }

    public override void Initialize(Application app, IWindow window) {
        if (app is not GameBreakerApplication)
            throw new ArgumentException("Application must be of type GameBreakerApplication.", nameof(app));

        base.Initialize(app, window);
    }
}
