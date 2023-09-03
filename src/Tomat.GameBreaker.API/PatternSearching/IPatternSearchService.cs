namespace Tomat.GameBreaker.API.PatternSearching;

public interface IPatternSearchService {
    bool FindByteArray(string pattern, string mask, out nuint address);

    bool FindByteArray(string pattern, string mask, nuint searchRegionBase, nuint searchRegionSize, out nuint address);
}
