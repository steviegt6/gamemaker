#pragma once

#include "coreclr_delegates.h"
#include "hostfxr.h"
#include "json.hpp"

inline hostfxr_initialize_for_runtime_config_fn init_fptr;
inline hostfxr_get_runtime_delegate_fn get_delegate_fptr;
inline hostfxr_close_fn close_fptr;
inline hostfxr_set_runtime_property_value_fn set_prop_fptr;

extern bool init_dotnet(const nlohmann::json& json, const std::wstring& cwd, const std::wstring& managed_host_dir);
extern bool start_clr();
extern bool load_hostfxr();
extern load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t* config_path, const char_t* base_directory);
extern void* load_library(const char_t* path);
extern void* get_export(void* h, const char* name);
