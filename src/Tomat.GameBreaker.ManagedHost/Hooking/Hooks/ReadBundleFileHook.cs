using System;
using System.Runtime.Versioning;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.Hooking.Hooks;
using Tomat.GameBreaker.API.PatternSearching;

namespace Tomat.GameBreaker.ManagedHost.Hooking.Hooks;

internal sealed class ReadBundleFileHook : IReadBundleFileHook {
    private readonly IPatternSearchService patternSearchService;

    public ReadBundleFileHook(IPatternSearchService patternSearchService) {
        this.patternSearchService = patternSearchService;
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public void CreateHook(IHookService hookService) {
        //var failed = patternSearchService.FindByteArray("\x45\x84\xe4\x00\x00\xe8\x00\x00\x00\x00\xeb", "xxxxxx????xx??????xx", out var addr);
        //Console.WriteLine($"success: {failed}, addr: {addr}");

        var a = patternSearchService.FindByteArray(new byte[] { 0xE8, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0xF0, 0x4D, 0x85, 0xFF }, "x????xxxxxx"u8.ToArray(), out var addr);
        Console.WriteLine($"{a} {addr}");
        
        // direct ref
        // address in opcode
        // if (!patternSearchService.FindByteArray("\xE8\x00\x00\x00\x00\x0F\xB6\xD8\x3C\x01", ))
    }

    public IReadBundleFileHook.Delegate? Original { get; set; }
}
