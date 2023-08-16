#include <string>
#include <windows.h>
#include "include/json.hpp"
#include "config.h"
#include "log.h"

nlohmann::json* init_config(const std::wstring& cwd)
{
    const std::wstring config_path = cwd + L"\\gamebreaker.json";

    msg(gray, "Config path: %ws\n", config_path.c_str());

    const DWORD config_file_attributes = GetFileAttributes(config_path.c_str());
    if (config_file_attributes == INVALID_FILE_ATTRIBUTES)
    {
        const DWORD error = GetLastError();

        if (error == ERROR_FILE_NOT_FOUND)
        {
            msg(light_red, "Config file not found.\n");
            return nullptr;
        }

        msg(light_red, "Failed to get config file with error code %d.\n", error);
        return nullptr;
    }

    if (config_file_attributes & FILE_ATTRIBUTE_DIRECTORY)
    {
        msg(light_red, "Config file is a directory.\n");
        return nullptr;
    }

    FILE* config_file;
    (void)_wfopen_s(&config_file, config_path.c_str(), L"r");
    if (config_file == nullptr)
    {
        msg(light_red, "Failed to open config file.\n");
        return nullptr;
    }

    // nlohmann::json* json = new nlohmann::json();

    auto json = nlohmann::json::parse(config_file);

    json["actAsUniprox"] = json.contains("actAsUniprox") ? json["actAsUniprox"].get<bool>() : false;

    msg(gray, "Loaded config:\n");
    msg(gray, "actAsUniprox: %s\n", json["actAsUniprox"].get<bool>() ? "true" : "false");

    return new nlohmann::json(json);
}
