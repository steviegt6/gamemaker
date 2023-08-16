#include <string>
#include <windows.h>
#include "config.h"
#include "log.h"

bool init_config(const std::wstring& cwd)
{
    const std::wstring config_path = cwd + L"\\gamebreaker.json";

    const DWORD config_file_attributes = GetFileAttributes(config_path.c_str());
    if (config_file_attributes == INVALID_FILE_ATTRIBUTES)
    {
        const DWORD error = GetLastError();

        if (error == ERROR_FILE_NOT_FOUND)
        {
            msg(light_red, "Config file not found.\n");
            return false;
        }

        msg(light_red, "Failed to get config file with error code %d.\n", error);
        return false;
    }

    if (config_file_attributes & FILE_ATTRIBUTE_DIRECTORY)
    {
        msg(light_red, "Config file is a directory.\n");
        return false;
    }

    return true;
}
