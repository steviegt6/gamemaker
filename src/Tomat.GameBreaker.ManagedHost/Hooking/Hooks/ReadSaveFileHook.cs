﻿using System;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.Hooking.Hooks;
using Tomat.GameBreaker.API.PatternSearching;
using Tomat.GameBreaker.API.Platform;
using IServiceProvider = Tomat.GameBreaker.API.DependencyInjection.IServiceProvider;

namespace Tomat.GameBreaker.ManagedHost.Hooking.Hooks;

internal sealed class ReadSaveFileHook : IReadSaveFileHook {
    public IReadSaveFileHook.Delegate? Original { get; set; }

    private readonly IPlatformService platform;
    private readonly IPatternSearchService patternSearcher;

    // ReSharper disable once NotAccessedField.Local
    private IReadSaveFileHook.Delegate? delegateHolder;

    public ReadSaveFileHook(IServiceProvider provider) {
        platform = provider.ExpectService<IPlatformService>();
        patternSearcher = provider.ExpectService<IPatternSearchService>();
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public unsafe void CreateHook(IHookService hookService) {
        if (platform.Is64Bit) {
            // This pattern is copied from ReadBundleFileHook, since
            // ReadSaveFile appears literally right after.
            var pattern = new byte[] {
                0x45, 0x84, 0xE4,             // TEST R12B, R12B
                0x74, 0x00,                   // JZ   <rel8>
                0xE8, 0x00, 0x00, 0x00, 0x00, // CALL <rel32>
                0xEB, 0x00,                   // JMP  <rel8>
                0xE8, 0x00, 0x00, 0x00, 0x00, // CALL <rel32>    (we want to hook this)
            };
            var patternMask = "xxxx?x????x?x????"u8.ToArray();

            if (!patternSearcher.FindByteArray(pattern, patternMask, out var address))
                throw new Exception("Failed to find pattern for ReadSaveFile hook.");

            Console.WriteLine("Found ReadSaveFile pattern at 0x{0:X}", address);
            Console.WriteLine("Pointing to byte: 0x{0:X}", *(byte*)address.ToPointer());

            var instructionBase = address + 12;
            var relative = *(int*)(instructionBase + 1).ToPointer();
            var eip = instructionBase + 5 + (nuint)relative;

            this.CreateHook(hookService, (nint)eip, delegateHolder = Hook);
        }
        else {
            // TODO: Support hooking on 32-bit systems.
            throw new PlatformNotSupportedException();
        }
    }

    private unsafe void* Hook(byte* a1, nuint* a2, void* a3) {
        Console.WriteLine("ReadSaveFile called with: {0}", AnsiStringMarshaller.ConvertToManaged(a1));
        // Console.WriteLine("ReadBundleFile returned: {0}", AnsiStringMarshaller.ConvertToManaged(ret));
        return Original!(a1, a2, a3);
    }
}
