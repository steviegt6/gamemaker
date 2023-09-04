using System.Runtime.InteropServices;
using System.Text;
using Tomat.GameBreaker.API.PatternSearching;

namespace Tomat.GameBreaker.ManagedHost.PatternSearching;

internal sealed unsafe partial class PatternSearchService : IPatternSearchService {
    // bool find_byte_array(byte* pattern, int pattern_length, byte* mask, int mask_length, uintptr_t region_base, uintptr_t region_size, uintptr_t* result)

    [LibraryImport("dbgcore.dll", EntryPoint = "find_byte_array")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FindByteArrayImpl(byte* pattern, int patternLength, byte* mask, int maskLength, nuint regionBase, nuint regionSize, out nuint result);

    public bool FindByteArray(byte[] pattern, byte[] mask, out nuint address) {
        return FindByteArray(pattern, mask, 0, 0, out address);
    }

    public bool FindByteArray(byte[] pattern, byte[] mask, nuint searchRegionBase, nuint searchRegionSize, out nuint address) {
        fixed (byte* pPattern = &pattern[0])
        fixed (byte* pMask = &mask[0])
            return FindByteArrayImpl(pPattern, pattern.Length, pMask, mask.Length, searchRegionBase, searchRegionSize, out address);
    }
}
