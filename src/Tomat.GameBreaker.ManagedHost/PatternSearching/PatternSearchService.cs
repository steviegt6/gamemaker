using System.Runtime.InteropServices;
using System.Text;
using Tomat.GameBreaker.API.PatternSearching;

namespace Tomat.GameBreaker.ManagedHost.PatternSearching;

internal sealed unsafe partial class PatternSearchService : IPatternSearchService {
    // bool find_byte_array(byte* pattern, int pattern_length, byte* mask, int mask_length, uintptr_t region_base, uintptr_t region_size, uintptr_t* result)

    [LibraryImport("dbgcore.dll", EntryPoint = "find_byte_array")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FindByteArrayImpl(byte* pattern, int patternLength, byte* mask, int maskLength, nuint regionBase, nuint regionSize, out nuint result);

    public bool FindByteArray(string pattern, string mask, out nuint address) {
        return FindByteArray(pattern, mask, 0, 0, out address);
    }

    public bool FindByteArray(string pattern, string mask, nuint searchRegionBase, nuint searchRegionSize, out nuint address) {
        var patternBytes = Encoding.UTF8.GetBytes(pattern);
        var maskBytes = Encoding.UTF8.GetBytes(mask);

        fixed (byte* pPattern = &patternBytes[0])
        fixed (byte* pMask = &maskBytes[0])
            return FindByteArrayImpl(pPattern, patternBytes.Length, pMask, maskBytes.Length, searchRegionBase, searchRegionSize, out address);
    }
}
