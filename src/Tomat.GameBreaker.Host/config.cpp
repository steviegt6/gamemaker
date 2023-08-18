#include "config.h"

#include <fstream>
#include <string>
#include <windows.h>

#include "json.hpp"
#include "log.h"

nlohmann::json* init_config(const std::wstring& cwd, const std::wstring& managed_host_dir)
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

    std::ifstream config_file(config_path.c_str());
    /*if (config_file == nullptr)
    {
        msg(light_red, "Failed to open config file.\n");
        return nullptr;
    }*/

    // nlohmann::json* json = new nlohmann::json();

    auto json = nlohmann::json::parse(config_file);

    json["actAsUniprox"] = json.contains("actAsUniprox") ? json["actAsUniprox"].get<bool>() : false;
    json["dotnet_runtimeconfig_path"] = json.contains("dotnet_runtimeconfig_path") ? json["dotnet_runtimeconfig_path"].get<std::string>() : "Tomat.GameBreaker.ManagedHost.runtimeconfig.json";
    json["dotnet_assembly_path"] = json.contains("dotnet_assembly_path") ? json["dotnet_assembly_path"].get<std::string>() : "Tomat.GameBreaker.ManagedHost.dll";
    json["dotnet_assembly_type_name"] = json.contains("dotnet_assembly_type_name") ? json["dotnet_assembly_type_name"].get<std::string>() : "Tomat.GameBreaker.ManagedHost.Program, Tomat.GameBreaker.ManagedHost";
    json["dotnet_assembly_method_name"] = json.contains("dotnet_assembly_method_name") ? json["dotnet_assembly_method_name"].get<std::string>() : "Main";

    msg(gray, "Loaded config: {\n");
    msg(gray, "    actAsUniprox: %s\n", json["actAsUniprox"].get<bool>() ? "true" : "false");
    msg(gray, "    dotnet_runtimeconfig_path: %s\n", json["dotnet_runtimeconfig_path"].get<std::string>().c_str());
    msg(gray, "    dotnet_assembly_path: %s\n", json["dotnet_assembly_path"].get<std::string>().c_str());
    msg(gray, "    dotnet_assembly_type_name: %s\n", json["dotnet_assembly_type_name"].get<std::string>().c_str());
    msg(gray, "    dotnet_assembly_method_name: %s\n", json["dotnet_assembly_method_name"].get<std::string>().c_str());
    msg(gray, "}\n");

    // msg(white, "For any relative dotnet paths, '%ws' will be used as the base directory, NOT the current working directory.\n", managed_host_dir.c_str());
    msg(white, "For any relative dotnet paths, '%ws' will be used as the base directory if the file is not resolved relative to the current working directory.\n", managed_host_dir.c_str());

    return new nlohmann::json(json);
}
