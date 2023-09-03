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

        var a = patternSearchService.FindByteArray("\xE8\x00\x00\x00\x00\x48\x8B\xF0\x4D\x85\xFF", "x????xxxxxx", out var addr);
        Console.WriteLine($"{a} {addr}");
    }

    public IReadBundleFileHook.Delegate? Original { get; set; }
}
