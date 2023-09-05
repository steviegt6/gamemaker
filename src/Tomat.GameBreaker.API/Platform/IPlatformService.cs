using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Platform;

public interface IPlatformService {
    bool Is64Bit { get; }

    OSPlatform OsPlatform { get; }
}
