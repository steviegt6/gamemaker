using System.Runtime.InteropServices;
using Tomat.GameBreaker.API.Platform;

namespace Tomat.GameBreaker.ManagedHost.Platform;

// Decided to use individual types for different combinations since there may
// eventually be different logic across bitness and operating system types. It's
// gross, cry about it.

internal abstract class SimplePlatform : IPlatformService {
    public abstract bool Is64Bit { get; }

    public abstract OSPlatform OsPlatform { get; }
}

internal abstract class WindowsPlatform : SimplePlatform {
    public override OSPlatform OsPlatform => OSPlatform.Windows;
}

internal sealed class WindowsPlatform64 : WindowsPlatform {
    public override bool Is64Bit => true;
}

internal sealed class WindowsPlatform32 : WindowsPlatform {
    public override bool Is64Bit => false;
}

internal abstract class OsxPlatform : SimplePlatform {
    public override OSPlatform OsPlatform => OSPlatform.OSX;
}

internal sealed class OsxPlatform64 : OsxPlatform {
    public override bool Is64Bit => true;
}

internal sealed class OsxPlatform32 : OsxPlatform {
    public override bool Is64Bit => false;
}

internal abstract class LinuxPlatform : SimplePlatform {
    public override OSPlatform OsPlatform => OSPlatform.Linux;
}

internal sealed class LinuxPlatform64 : LinuxPlatform {
    public override bool Is64Bit => true;
}

internal sealed class LinuxPlatform32 : LinuxPlatform {
    public override bool Is64Bit => false;
}
