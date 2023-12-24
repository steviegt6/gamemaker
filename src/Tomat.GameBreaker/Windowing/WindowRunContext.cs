using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;

namespace Tomat.GameBreaker.Windowing;

public sealed class WindowRunContext {
    public ImGuiWindow ImGuiWindow { get; }

    public bool Initialized { get; private set; }

    public bool Disposed { get; private set; }

    public bool Running => Initialized && !Disposed;

    private Sdl2Window SdlWindow => ImGuiWindow.Window;

    private GraphicsDevice Gd => ImGuiWindow.GraphicsDevice;

    private CommandList Cl => ImGuiWindow.CommandList!;

    private ImGuiRenderer? renderer;

    public WindowRunContext(ImGuiWindow imGuiWindow) {
        ImGuiWindow = imGuiWindow;
    }

    public void Run() {
        if (!Initialized) {
            Initialized = true;

            ImGuiWindow.Initialize();
            InitializeImGui();
        }

        while (ImGuiWindow.Window.Exists) {
            var input = SdlWindow.PumpEvents();

            if (!SdlWindow.Exists)
                break;

            // TODO: Calculate actual delta time.
            UpdateImGui(1f / 60f, input);
            ImGuiWindow.Update();

            var io = ImGui.GetIO();
            if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0) {
                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault();
            }

            Cl.Begin();
            Cl.SetFramebuffer(Gd.SwapchainFramebuffer);
            Cl.ClearColorTarget(0, RgbaFloat.Clear);
            RenderImGui();
            Cl.End();
            Gd.SubmitCommands(Cl);
            Gd.SwapBuffers(Gd.MainSwapchain);
        }

        Disposed = true;
        ImGuiWindow.Dispose();
        renderer!.Dispose();
    }

    private void InitializeImGui() {
        renderer = new ImGuiRenderer(Gd, Gd.SwapchainFramebuffer.OutputDescription, (int)Gd.SwapchainFramebuffer.Width, (int)Gd.SwapchainFramebuffer.Height);

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

        renderer.RecreateFontDeviceTexture();
    }

    private void UpdateImGui(float delta, InputSnapshot input) {
        renderer!.Update(delta, input);
    }

    private void RenderImGui() {
        renderer!.Render(Gd, Cl);
    }

    private void ResizeImGuiViewport(int width, int height) {
        renderer!.WindowResized(width, height);
    }
}
