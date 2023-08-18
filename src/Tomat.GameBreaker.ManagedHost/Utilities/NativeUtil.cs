using System.Text;

namespace Tomat.GameBreaker.ManagedHost.Utilities; 

internal static class NativeUtil {
    public static unsafe string ReadWCharPtr(short* ptr) {
        var sb = new StringBuilder();
        
        while (*ptr != 0) {
            sb.Append((char) *ptr);
            ptr++;
        }
        
        return sb.ToString();
    }
}
