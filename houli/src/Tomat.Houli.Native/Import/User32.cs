using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static partial class User32 {
    [LibraryImport("user32.dll", EntryPoint = "MessageBoxW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    internal static partial int MessageBox(nint hWnd, string text, string caption, uint type);
}
