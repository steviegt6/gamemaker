using Silk.NET.Maths;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Windowing;

namespace Tomat.GameBreaker.Windows.Main;

internal sealed class MainWindow : ImGuiControlledWindow {
    public MainWindow(Application app, ref WindowOptions options) : base(app, ref options) {
        options = options with {
            Size = new Vector2D<int>(1280, 720),
            Title = "GameBreaker",
        };
    }

    protected override void RenderImGuiWindow(double delta) { }
}
