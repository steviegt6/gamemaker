#include <string>
#include <windows.h>

#include "config.h"
#include "console.h"
#include "dotnet.h"

// Proxying technique taken from Archie's Uniprox project.
// https://github.com/Archie-osu/Uniprox
// GNU General Public License, version 3

void thread_main(HMODULE)
{
    TCHAR cwd_buf[MAX_PATH];
    GetModuleFileName(nullptr, cwd_buf, MAX_PATH);
    std::wstring cwd(cwd_buf);
    const std::wstring::size_type separator = cwd.find_last_of(L"\\/");
    cwd = cwd.substr(0, separator);

    init_console();

    if (!init_config(cwd))
    {
        MessageBox(nullptr, L"Failed to initialize config, cancelling injection.", L"Tomat.GameBreaker.Host", MB_OK | MB_ICONERROR);
        return;
    }

    init_dotnet();
}

// ReSharper disable once CppInconsistentNaming
BOOL APIENTRY DllMain(const HMODULE instance, const DWORD reason, LPVOID)
{
    if (reason != DLL_PROCESS_ATTACH)
        return true;

    CloseHandle(CreateThread(nullptr, 0, reinterpret_cast<LPTHREAD_START_ROUTINE>(thread_main), instance, 0, nullptr));
    return true;
}

extern "C" __declspec(dllexport) BOOL MiniDumpReadDumpStream(PVOID, ULONG, void*, void*, void*)
{
    return false;
}

extern "C" __declspec(dllexport) BOOL MiniDumpWriteDump(HANDLE, DWORD, HANDLE, DWORD, void*, void*, void*)
{
    return false;
}
