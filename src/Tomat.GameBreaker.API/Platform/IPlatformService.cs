using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Platform;

public interface IPlatformService {
    bool Is64Bit { get; }

    OSPlatform OsPlatform { get; }

    bool IsSuspended(Process process);

    void Restart(Process process, string[] dllPaths);

    void Resume(Process process);
}
