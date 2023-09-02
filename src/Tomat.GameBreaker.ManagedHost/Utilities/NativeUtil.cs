using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Windows.Win32;

namespace Tomat.GameBreaker.ManagedHost.Utilities;

internal static class NativeUtil {
    private struct CModule {
#pragma warning disable CS0649
        public uint Base;
        public nint Size;
        public uint EntryPoint;
#pragma warning restore CS0649
    }

    public static unsafe string ReadWCharPtr(short* ptr) {
        var sb = new StringBuilder();

        while (*ptr != 0) {
            sb.Append((char) *ptr);
            ptr++;
        }

        return sb.ToString();
    }

    private delegate int GetModuleInformationDelegate(nint processId, nint moduleHandle, out nint pOutModule, int size);

    [SupportedOSPlatform("windows5.1.2600")]
    private static unsafe void GetModuleInformation(string? moduleName, out CModule? outModule) {
        Debugger.Launch();
        Debugger.Break();
        var module = PInvoke.GetModuleHandle("kernel32.dll");
        var getModuleInformation = PInvoke.GetProcAddress(module, "K32GetModuleInformation");
        var fn = getModuleInformation.CreateDelegate<GetModuleInformationDelegate>();

        var moduleHandle = PInvoke.GetModuleHandle(moduleName);

        if (moduleHandle.IsInvalid) {
            outModule = null;
            return;
        }

        fn(Environment.ProcessId, moduleHandle.DangerousGetHandle(), out var pOutModule, sizeof(CModule));
        outModule = Marshal.PtrToStructure<CModule>(pOutModule);
        return;
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public static unsafe bool FindBytePattern(byte[] pattern, string mask, nint regionBase, nint regionSize, out nint address) {
        address = 0;

        if (regionBase == 0 && regionSize == 0) {
            GetModuleInformation(null, out var moduleInfo);
            if (!moduleInfo.HasValue)
                return false;

            regionBase = (nint)moduleInfo.Value.Base;
            regionSize = moduleInfo.Value.Size;
        }

        for (var i = 0; i < regionSize - mask.Length; i++) {
            var found = true;

            for (var j = 0; j < mask.Length; j++) {
                var maskChar = mask[j];
                if (maskChar == '?')
                    continue;

                found &= *(byte*)new nint(regionBase + i + j).ToPointer() == pattern[j];
            }

            if (!found)
                continue;

            // TODO: stringSearch && base + i - 1 != null? not relevant?

            address = regionBase + i;
            return true;
        }

        return false;
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public static nint? FindPattern(byte[] pattern, string mask, nint searchRegionBase = 0, nint searchRegionSize = 0) {
        if (!FindBytePattern(pattern, mask, searchRegionBase, searchRegionSize, out var address))
            return null;

        return address;
    }
}
