using System.Diagnostics;
using System.Runtime.InteropServices;
using Tomat.GameBreaker.API.Platform;

namespace Tomat.GameBreaker.ManagedHost.Platform;

internal class LinuxPlatform : IPlatformService {
    public OSPlatform OsPlatform => OSPlatform.Linux;

    public bool Is64Bit { get; }

    public LinuxPlatform(bool is64Bit) {
        Is64Bit = is64Bit;
    }

    public bool IsSuspended(Process process) {
        throw new System.NotImplementedException();
    }

    public void Restart(Process process, string[] dllPaths) {
        throw new System.NotImplementedException();
    }

    public void Resume(Process process) {
        throw new System.NotImplementedException();
    }
}
