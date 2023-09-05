using System;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using Tomat.GameBreaker.API.DependencyInjection;
using Tomat.GameBreaker.API.FileModification;
using Tomat.GameBreaker.API.Hooking;
using Tomat.GameBreaker.API.Hooking.Hooks;
using Tomat.GameBreaker.API.PatternSearching;
using Tomat.GameBreaker.API.Platform;
using IServiceProvider = Tomat.GameBreaker.API.DependencyInjection.IServiceProvider;

namespace Tomat.GameBreaker.ManagedHost.Hooking.Hooks;

internal sealed class ReadBundleFileHook : IReadBundleFileHook {
    public IReadBundleFileHook.Delegate? Original { get; set; }

    private readonly IPlatformService platform;
    private readonly IPatternSearchService patternSearcher;
    private readonly IFileModifierService fileModifier;

    // ReSharper disable once NotAccessedField.Local
    private IReadBundleFileHook.Delegate? delegateHolder;

    public ReadBundleFileHook(IServiceProvider provider) {
        platform = provider.ExpectService<IPlatformService>();
        patternSearcher = provider.ExpectService<IPatternSearchService>();
        fileModifier = provider.ExpectService<IFileModifierService>();
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public unsafe void CreateHook(IHookService hookService) {
        if (platform.Is64Bit) {
            var pattern = new byte[] {
                0x45, 0x84, 0xE4,             // TEST R12B, R12B
                0x74, 0x00,                   // JZ   <rel8>
                0xE8, 0x00, 0x00, 0x00, 0x00, // CALL <rel32>    (we want to hook this)
                0xEB, 0x00,                   // JMP  <rel8>
            };
            var patternMask = "xxxx?x????x?"u8.ToArray();

            if (!patternSearcher.FindByteArray(pattern, patternMask, out var address))
                throw new Exception("Failed to find pattern for ReadBundleFile hook.");

            Console.WriteLine("Found ReadBundleFile pattern at 0x{0:X}", address);
            Console.WriteLine("Pointing to byte: 0x{0:X}", *(byte*)address.ToPointer());

            var instructionBase = address + 5;
            var relative = *(int*)(instructionBase + 1).ToPointer();
            var eip = instructionBase + 5 + (nuint)relative;

            this.CreateHook(hookService, (nint)eip, delegateHolder = Hook);
        }
        else {
            // TODO: Support hooking on 32-bit systems.
            throw new PlatformNotSupportedException();
        }
    }

    private unsafe byte* Hook(byte* a1, nuint* a2) {
        // Expecting a1 to never be null here... let's hope.
        var path = AnsiStringMarshaller.ConvertToManaged(a1)!;
        Console.WriteLine("ReadBundleFile called with: {0}", path);

        if (fileModifier.TryModifyFile(path, FileContext.Save, out var newPath)) {
            Console.WriteLine("ReadBundleFile redirected to: {0}", newPath);
            return Original!(AnsiStringMarshaller.ConvertToUnmanaged(newPath), a2);
            // return AnsiStringMarshaller.ConvertToUnmanaged(File.ReadAllText(newPath));
        }

        return Original!(a1, a2);
    }
}
