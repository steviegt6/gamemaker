using System;
using Tomat.GameBreaker.Windowing;

namespace Tomat.GameBreaker.Platforms;

public sealed class Platform : IPlatform {
    private static readonly IPlatform platform = new Platform();

    public static void MakeWindowTransparent(ImGuiWindow window) {
        platform.MakeWindowTransparent(window);
    }

    #region Impl
    private readonly IPlatform impl = OperatingSystem.IsWindows()
        ? new WindowsPlatform()
        : OperatingSystem.IsMacOS()
            ? new MacPlatform()
            : OperatingSystem.IsLinux()
                ? new LinuxPlatform()
                : throw new PlatformNotSupportedException();

    void IPlatform.MakeWindowTransparent(ImGuiWindow window) {
        impl.MakeWindowTransparent(window);
    }
    #endregion
}
