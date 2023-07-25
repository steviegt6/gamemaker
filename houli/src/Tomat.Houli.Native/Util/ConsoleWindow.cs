using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Tomat.Houli.Native.Import;

namespace Tomat.Houli.Native.Util;

internal static class ConsoleWindow {
    private const uint generic_write = 0x40000000;
    private const uint generic_read = 0x80000000;
    private const uint file_share_read = 0x00000001;
    private const uint file_share_write = 0x00000002;
    private const uint open_existing = 0x00000003;
    private const uint file_attribute_normal = 0x80;

    internal static bool Initialize() {
        if (!Alloc()) {
            var error = Marshal.GetLastWin32Error();
            User32.MessageBox(nint.Zero, $"Failed to allocate console with error code: {error:x8}", "Houli Early Init Error", 0);
            return false;
        }

        var stdIn = CreateStream("CONIN$", generic_read, file_share_read);
        var stdOut = CreateStream("CONOUT$", generic_write, file_share_write);
        var stdErr = CreateStream("CONOUT$", generic_write, file_share_write);
        Console.SetIn(new StreamReader(stdIn));
        Console.SetOut(new StreamWriter(stdOut) {
            AutoFlush = true,
        });
        Console.SetError(new StreamWriter(stdErr) {
            AutoFlush = true,
        });

        var title = $"Tomat.Houli {(Environment.Is64BitProcess ? "x64" : "x86")} Console";
        Kernel32.SetConsoleTitle(title);

        // TODO: Disable left-click to select BS.

        return true;
    }

    private static bool Alloc() {
        return Kernel32.AllocConsole();
    }

    private static FileStream CreateStream(string name, uint access, uint share) {
        var pFile = new SafeFileHandle(Kernel32.CreateFile(name, access, share, nint.Zero, open_existing, file_attribute_normal, nint.Zero), true);
        return new FileStream(pFile, access == generic_read ? FileAccess.Read : FileAccess.Write);
    }
}
