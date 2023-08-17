#include <string>
#include <windows.h>

#include "config.h"
#include "console.h"
#include "dotnet.h"
#include "log.h"

// Proxying technique taken from Archie's Uniprox project.
// https://github.com/Archie-osu/Uniprox
// GNU General Public License, version 3

void load_uniprox_dlls(const std::wstring& cwd)
{
    // msg(light_gray, "Loading Uniprox DLLs...\n");

    const std::wstring uniprox_dir_path = cwd + L"\\uniprox";

    msg(light_gray, "Loading Uniprox DLLs from %S...\n", uniprox_dir_path.c_str());

    for (std::error_code error; const auto& file : std::filesystem::directory_iterator(uniprox_dir_path, error))
    {
        if (error)
        {
            msg(light_red, "Failed to iterate directory %s with error code %d.\n", uniprox_dir_path.c_str(), error.value());
            return;
        }

        if (!file.is_regular_file(error))
            continue;

        if (!file.path().has_filename())
            continue;

        if (!file.path().filename().has_extension())
            continue;

        if (!file.path().filename().extension().compare(".dll"))
        {
            msg(light_gray, "Loading %S...\n", file.path().c_str());
            if (const HMODULE module = LoadLibrary(file.path().c_str()); module == nullptr)
            {
                msg(light_red, "Failed to load %s with error code %d.\n", file.path().c_str(), GetLastError());
                return;
            }
        }
    }
}

DWORD thread_main(LPVOID)
{
    TCHAR cwd_buf[MAX_PATH];
    GetModuleFileName(nullptr, cwd_buf, MAX_PATH);
    std::wstring cwd(cwd_buf);
    const std::wstring::size_type separator = cwd.find_last_of(L"\\/");
    cwd = cwd.substr(0, separator);

    init_console();

    size_t required_size;
    _wgetenv_s(&required_size, nullptr, 0, L"APPDATA");
    std::wstring appdata_dir(required_size, L'\0');
    _wgetenv_s(&required_size, appdata_dir.data(), required_size, L"APPDATA");
    appdata_dir.resize(required_size - 1); // Remove null terminator.
    const std::wstring gamebreaker_dir = appdata_dir + L"\\gamebreaker";
    const std::wstring managed_host_dir = gamebreaker_dir + L"\\managed_host";

    const nlohmann::json* p_json = init_config(cwd, managed_host_dir);
    if (!p_json)
    {
        MessageBox(nullptr, L"Failed to initialize config, cancelling injection.", L"Tomat.GameBreaker.Host", MB_OK | MB_ICONERROR);
        return 0;
    }

    if (!std::filesystem::exists(managed_host_dir))
    {
        msg(yellow, "Managed host directory doesn't exist, this may cause failures.");
        return 0;
    }

    const nlohmann::json json = *p_json;

    init_dotnet();

    if (json["actAsUniprox"].get<bool>())
        load_uniprox_dlls(cwd);

    delete p_json;

    while (true)
        Sleep(1000);

    // return 0;
}

// ReSharper disable once CppInconsistentNaming
BOOL APIENTRY DllMain(const HMODULE instance, const DWORD reason, LPVOID)
{
    if (reason != DLL_PROCESS_ATTACH)
        return true;

    CloseHandle(CreateThread(nullptr, 0, thread_main, instance, 0, nullptr));
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
