#include "dotnet.h"

#include <windows.h>

#include "log.h"
#include "nethost.h"
#include "include/json.hpp"

std::wstring find_valid_path(std::string path, const std::wstring& cwd, const std::wstring& managed_host_dir, bool& found)
{
    found = true;

    const std::filesystem::path relative_path(path);
    if (relative_path.is_absolute())
        return relative_path.wstring();

    const std::wstring w_path(path.begin(), path.end());

    const std::filesystem::path cwd_path(cwd + L"\\" + w_path);
    if (exists(cwd_path))
        return cwd_path.wstring();

    const std::filesystem::path managed_host_dir_path(managed_host_dir + L"\\" + w_path);
    if (exists(managed_host_dir_path))
        return managed_host_dir_path.wstring();

    found = false;

    msg(light_red, "Failed to find valid path for %s. Tried:\n", path.c_str());
    msg(light_red, "    %ws\n", relative_path.c_str());
    msg(light_red, "    %ws\n", cwd_path.c_str());
    msg(light_red, "    %ws\n", managed_host_dir_path.c_str());
    return L"";
}

bool init_dotnet(const nlohmann::json& json, const std::wstring& cwd, const std::wstring& managed_host_dir, native_entry* entry)
{
    if (!std::filesystem::exists(managed_host_dir))
        msg(yellow, "Managed host directory doesn't exist, this may cause failures.\n");

    bool found_runtimeconfig_path = false;
    bool found_assembly_path = false;
    const std::wstring runtimeconfig_path = find_valid_path(json["dotnet_runtimeconfig_path"].get<std::string>(), cwd, managed_host_dir, found_runtimeconfig_path);
    const std::wstring assembly_path = find_valid_path(json["dotnet_assembly_path"].get<std::string>(), cwd, managed_host_dir, found_assembly_path);
    const std::string assembly_type_name = json["dotnet_assembly_type_name"].get<std::string>();
    const std::string assembly_method_name = json["dotnet_assembly_method_name"].get<std::string>();
    const std::wstring wide_assembly_type_name(assembly_type_name.begin(), assembly_type_name.end());
    const std::wstring wide_assembly_method_name(assembly_method_name.begin(), assembly_method_name.end());

    if (!found_runtimeconfig_path || !found_assembly_path)
    {
        msg(light_red, "Failed to find valid paths for dotnet runtimeconfig and/or assembly.\n");
        return false;
    }

    if (!load_hostfxr())
    {
        msg(light_red, "Failed to load hostfxr.\n");
        return false;
    }

    const load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer = get_dotnet_load_assembly(runtimeconfig_path.c_str(), cwd.c_str());
    if (!load_assembly_and_get_function_pointer)
    {
        msg(light_red, "Failed to get load_assembly_and_get_function_pointer.\n");
        return false;
    }


    native_entry entrypoint = nullptr;
    const int rc = load_assembly_and_get_function_pointer(
        assembly_path.c_str(),
        wide_assembly_type_name.c_str(),
        wide_assembly_method_name.c_str(),
        UNMANAGEDCALLERSONLY_METHOD,
        nullptr,
        reinterpret_cast<void**>(&entrypoint)
    );

    if (rc != 0 || !entrypoint)
    {
        msg(light_red, "Failed to load assembly with error code %x (%d).\n", rc, rc);
        return false;
    }

    *entry = entrypoint;
    return true;
}

bool load_hostfxr()
{
    char_t buffer[MAX_PATH];
    size_t buffer_size = sizeof(buffer) / sizeof(char_t);
    const int rc = get_hostfxr_path(buffer, &buffer_size, nullptr);
    if (rc != 0)
    {
        msg(light_red, "Failed to get hostfxr path with error code %x (%d).\n", rc, rc);
        return false;
    }

    msg(light_gray, "Loading hostfxr from %ls...\n", buffer);

    void* lib = load_library(buffer);
    init_fptr = static_cast<hostfxr_initialize_for_runtime_config_fn>(get_export(lib, "hostfxr_initialize_for_runtime_config")); // NOLINT(clang-diagnostic-microsoft-cast)
    get_delegate_fptr = static_cast<hostfxr_get_runtime_delegate_fn>(get_export(lib, "hostfxr_get_runtime_delegate")); // NOLINT(clang-diagnostic-microsoft-cast)
    close_fptr = static_cast<hostfxr_close_fn>(get_export(lib, "hostfxr_close")); // NOLINT(clang-diagnostic-microsoft-cast)
    set_prop_fptr = static_cast<hostfxr_set_runtime_property_value_fn>(get_export(lib, "hostfxr_set_runtime_property_value")); // NOLINT(clang-diagnostic-microsoft-cast)

    return lib && init_fptr && get_delegate_fptr && close_fptr && set_prop_fptr;
}

load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t* config_path, const char_t* base_directory)
{
    void* load_assembly_and_get_function_pointer = nullptr;
    hostfxr_handle ctx = nullptr;
    int rc = init_fptr(config_path, nullptr, &ctx);
    if (rc != 0 || ctx == nullptr)
    {
        msg(light_red, "Failed to initialize hostfxr with error code %x (%d).\n", rc, rc);
        close_fptr(ctx);
        return nullptr;
    }

    rc = get_delegate_fptr(ctx, hdt_load_assembly_and_get_function_pointer, &load_assembly_and_get_function_pointer);

    if (rc != 0 || load_assembly_and_get_function_pointer == nullptr)
    {
        msg(light_red, "Failed to get load_assembly_and_get_function_pointer delegate with error code %x (%d).\n", rc, rc);
        // close_fptr(ctx);
        return nullptr;
    }

    set_prop_fptr(ctx, L"APP_CONTEXT_BASE_DIRECTORY", base_directory);
    close_fptr(ctx);
    return static_cast<load_assembly_and_get_function_pointer_fn>(load_assembly_and_get_function_pointer); // NOLINT(clang-diagnostic-microsoft-cast)
}


void* load_library(const char_t* path)
{
    const HMODULE h = LoadLibraryW(path);
    if (!h)
    {
        msg(light_red, "Failed to load library %ls with error code %d.\n", path, GetLastError());
        return nullptr;
    }
    return h;
}

void* get_export(void* h, const char* name)
{
    void* f = GetProcAddress(static_cast<HMODULE>(h), name); // NOLINT(clang-diagnostic-microsoft-cast)
    if (!f)
    {
        msg(light_red, "Failed to get export %s with error code %d.\n", name, GetLastError());
        return nullptr;
    }
    return f;
}
