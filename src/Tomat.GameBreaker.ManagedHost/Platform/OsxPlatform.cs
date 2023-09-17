using System.Diagnostics;
using System.Runtime.InteropServices;
using Tomat.GameBreaker.API.Platform;

namespace Tomat.GameBreaker.ManagedHost.Platform;

internal class OsxPlatform : IPlatformService {
    public OSPlatform OsPlatform => OSPlatform.OSX;

    public bool Is64Bit { get; }

    public OsxPlatform(bool is64Bit) {
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
