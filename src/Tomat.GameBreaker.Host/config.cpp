#include <string>
#include <windows.h>
#include "config.h"

bool init_config(const std::wstring& cwd)
{
    const std::wstring config_path = cwd + L"\\gamebreaker.json";

    const DWORD config_file_attributes = GetFileAttributes(config_path.c_str());
    if (config_file_attributes == INVALID_FILE_ATTRIBUTES)
    {
        const DWORD error = GetLastError();

        if (error == ERROR_FILE_NOT_FOUND)
        {
            MessageBox(nullptr, L"Config file not found.", L"GameBreaker", MB_OK | MB_ICONERROR);
            return false;
        }

        MessageBox(nullptr, (L"Failed to get config file with error code " + std::to_wstring(error) + L".").c_str(), L"GameBreaker", MB_OK | MB_ICONERROR);
        return false;
    }

    if (config_file_attributes & FILE_ATTRIBUTE_DIRECTORY)
    {
        MessageBox(nullptr, L"Config file is a directory.", L"GameBreaker", MB_OK | MB_ICONERROR);
        return false;
    }

    return true;
}
