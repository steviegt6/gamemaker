namespace Tomat.GameBreaker.API.PatternSearching;

public interface IPatternSearchService {
    bool FindByteArray(byte[] pattern, byte[] mask, out nuint address);

    bool FindByteArray(byte[] pattern, byte[] mask, nuint searchRegionBase, nuint searchRegionSize, out nuint address);
}
