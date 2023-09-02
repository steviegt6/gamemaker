using System.Runtime.Versioning;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.Hooking.Hooks;
using Tomat.GameBreaker.ManagedHost.Utilities;

namespace Tomat.GameBreaker.ManagedHost.Hooking.Hooks; 

internal sealed class ReadBundleFileHook : IReadBundleFileHook {
    [SupportedOSPlatform("windows5.1.2600")]
    public void CreateHook(IHookService hookService) {
        var addr = NativeUtil.FindPattern(new byte[] { 0x45, 0x84, 0xe4, 0x00, 0x00, 0xe8, 0x00, 0x00, 0x00, 0x00, 0xeb }, "xxxxxx????xx??????xx");
    }

    public IReadBundleFileHook.Delegate? Original { get; set; }
}
