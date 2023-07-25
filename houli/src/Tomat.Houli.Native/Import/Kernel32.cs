using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static partial class Kernel32 {
    [LibraryImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool AllocConsole();

    [LibraryImport("kernel32.dll", EntryPoint = "SetConsoleTitleW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetConsoleTitle(string lpConsoleTitle);

    [LibraryImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    internal static partial nint CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, nint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, nint hTemplateFile);

    [LibraryImport("kernel32.dll", EntryPoint = "LoadLibraryW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    internal static partial nint LoadLibrary(string lpFileName);

    [LibraryImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GetProcAddress(nint hModule, string lpProcName);
}
