using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;
using Windows.Win32.System.Threading;
using Tomat.GameBreaker.API.Platform;

namespace Tomat.GameBreaker.ManagedHost.Platform;

internal partial class WindowsPlatform : IPlatformService {
    public OSPlatform OsPlatform => OSPlatform.Windows;

    public bool Is64Bit { get; }

    public WindowsPlatform(bool is64Bit) {
        Is64Bit = is64Bit;
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public bool IsSuspended(Process process) {
        // TODO: It's reasonably possible to do this comfortably in native code,
        // but this is C# and I'm lazy. Let's just... indicate it with a command
        // line flag.
        /*var suspended = true;
        foreach (ProcessThread thread in process.Threads) {
            var threadHandle = PInvoke.OpenThread(THREAD_ACCESS_RIGHTS.THREAD_ALL_ACCESS, false, (uint)thread.Id);
            if (threadHandle.IsNull)
                continue;

            PInvoke.CloseHandle(threadHandle);
            suspended &= thread.ThreadState == ThreadState.Wait;
        }

        return suspended;*/

        var commandLine = GetCommandLine(process.Id);
        return commandLine.Contains("--suspended");
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public unsafe void Restart(Process process, string[] dllPaths) {
        var commandLine = GetCommandLine(process.Id);
        commandLine += " --suspended";
        commandLine += '\0';
        var cliSpan = new Span<char>(commandLine.ToCharArray());

        var dir = Directory.GetCurrentDirectory();
        STARTUPINFOW sInfo = default;
        PInvoke.CreateProcess(null, ref cliSpan, null, null, false, PROCESS_CREATION_FLAGS.CREATE_SUSPENDED, null, dir, in sInfo, out var pInfo);

        foreach (var dllPath in dllPaths) {
            var kernel32 = PInvoke.GetModuleHandle("kernel32.dll");
            var loadLibrary = PInvoke.GetProcAddress(kernel32, "LoadLibraryW");
            var allocPath = PInvoke.VirtualAllocEx(pInfo.hProcess, null, (nuint)(dllPath.Length + 1), VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT | VIRTUAL_ALLOCATION_TYPE.MEM_RESERVE, PAGE_PROTECTION_FLAGS.PAGE_READWRITE);

            fixed (char* pDllPath = dllPath)
                PInvoke.WriteProcessMemory(pInfo.hProcess, allocPath, pDllPath, (nuint)(dllPath.Length + 1), null);

            PInvoke.CreateRemoteThread(pInfo.hProcess, null, 0, loadLibrary.CreateDelegate<LPTHREAD_START_ROUTINE>(), allocPath, 0, null);
        }

        Environment.Exit(0);
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public void Resume(Process process) {
        foreach (ProcessThread thread in process.Threads) {
            var threadHandle = PInvoke.OpenThread(THREAD_ACCESS_RIGHTS.THREAD_SUSPEND_RESUME, false, (uint)thread.Id);
            if (threadHandle.IsNull)
                continue;

            uint suspendCount;

            do {
                suspendCount = PInvoke.ResumeThread(threadHandle);
            }
            while (suspendCount > 0);

            PInvoke.CloseHandle(threadHandle);
        }
    }

    // This evil code is provided by your local StackOverflow post:
    // https://stackoverflow.com/a/70322609
    [SupportedOSPlatform("windows5.1.2600")]
    private static unsafe string GetCommandLine(int procId) {
        var procHandle = PInvoke.OpenProcess((PROCESS_ACCESS_RIGHTS)0x410, false, (uint)procId);

        try {
            if (procHandle.IsNull)
                throw new InvalidOperationException("Attempted to retrieve command line data from an unresolved process.");

            var mem = stackalloc nint[sizeof(nint) * 16];
            int length;

            if (NtQueryInformationProcess(procHandle.Value, 0, mem, sizeof(nint) * 6, &length) != 0) {
                PInvoke.CloseHandle(procHandle);
                throw new InvalidOperationException("Could not query NT process information.");
            }

            var pbiBaseAddress = mem[1];

            if (pbiBaseAddress == 0)
                throw new InvalidDataException("pbi base address should not be null.");

            nuint read;
            if (!PInvoke.ReadProcessMemory(procHandle, (void*)pbiBaseAddress, mem, (nuint)(sizeof(nint) * 5), &read) || (int)read != sizeof(nint) * 5)
                throw new InvalidOperationException("ReadProcessMemory failed or a read underflow occurred!");

            var processParameters = mem[4];
            if (!PInvoke.ReadProcessMemory(procHandle, (void*)processParameters, mem, (nuint)(sizeof(nint) * 16), &read) || (int)read != sizeof(nint) * 16)
                throw new InvalidOperationException("ReadProcessMemory failed or a read underflow occurred!");

            var cmdLineUnicode = mem + 14;
            var cmdLineLength = ((short*)cmdLineUnicode)[1];
            var pStr = Marshal.AllocHGlobal(cmdLineLength);
            if (!PInvoke.ReadProcessMemory(procHandle, (void*)*(cmdLineUnicode + 1), (void*)pStr, (nuint)cmdLineLength, &read))
                throw new InvalidOperationException("ReadProcessMemory failed!");

            var str = new string((char*)pStr);
            Marshal.FreeHGlobal(pStr);
            return str;
        }
        finally {
            PInvoke.CloseHandle(procHandle);
        }
    }

    [LibraryImport("ntdll.dll", EntryPoint = "NtQueryInformationProcess")]
    private static unsafe partial int NtQueryInformationProcess(nint processHandle, int processInformationClass, void* processInformation, int processInformationLength, int* returnLength);
}
