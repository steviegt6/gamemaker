using ImGuiNET;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Windowing;

namespace Tomat.GameBreaker.Windows;

/// <summary>
///     A window that appears borderless and transparent, allowing ImGui to
///     control windowing.
/// </summary>
internal abstract class ImGuiControlledWindow : GameBreakerWindow {
    public bool UseImGuiWindowing => !App.Settings.UseNativeWindowing;

    protected ImGuiControlledWindow(Application app, ref WindowOptions options) : base(app, ref options) {
        if (!UseImGuiWindowing)
            return;

        options = options with {
            WindowBorder = WindowBorder.Hidden,
            TransparentFramebuffer = true,
        };
    }

    protected sealed override void Render(double delta) {
        base.Render(delta);

        ImGui.SetNextWindowSize(ImGui.GetMainViewport().Size);
        ImGui.SetNextWindowPos(ImGui.GetMainViewport().WorkPos);

        if (ImGui.Begin(Window.Title, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            RenderImGuiWindow(delta);

        ImGui.End();
    }

    protected abstract void RenderImGuiWindow(double delta);
}
