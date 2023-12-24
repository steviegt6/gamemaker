using System.Runtime.InteropServices.Marshalling;
using Silk.NET.GLFW;
using Tomat.GameBreaker.Logging;

namespace Tomat.GameBreaker.Windowing.GLFW;

internal sealed unsafe class GlfwWindowProvider : IWindowProvider {
    private readonly Glfw glfw = Glfw.GetApi();

    public bool Initialize(Logger logger) {
        logger = logger.AsChild<GlfwWindowProvider>();

        logger.Debug("Initializing GLFW...");
        if (glfw.Init()) {
            glfw.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            glfw.WindowHint(WindowHintInt.ContextVersionMinor, 6);
            glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            return true;
        }

        var code = glfw.GetError(out var desc);
        logger.Error($"Failed to initialize GLFW: {Utf8StringMarshaller.ConvertToManaged(desc)} (0x{code:X8})");
        return false;
    }

    public void Terminate(Logger logger) {
        logger = logger.AsChild<GlfwWindowProvider>();

        logger.Debug($"Terminating {nameof(GlfwWindowProvider)}...");
        glfw.Terminate();
    }

    public T? CreateWindow<T>(Logger logger, T window) where T : ImGuiWindow {
        logger = logger.AsChild<GlfwWindowProvider>();

        logger.Debug($"Creating GLFW window \"{window.InternalName}\"...");
        var windowHandle = glfw.CreateWindow(window.InitialWidth, window.InitialHeight, window.InternalName, null, null);

        if (windowHandle == null) {
            logger.Error($"Failed to create window \"{window.InternalName}\"!");
            return null;
        }

        window.Window = new GlfwWindow(windowHandle, glfw);
        return window;
    }
}
