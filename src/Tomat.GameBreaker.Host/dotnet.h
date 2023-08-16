#pragma once

#include "include/hostfxr.h"
#include "include/coreclr_delegates.h"

extern hostfxr_initialize_for_runtime_config_fn init_fptr;
extern hostfxr_get_runtime_delegate_fn get_delegate_fptr;
extern hostfxr_close_fn close_fptr;
extern hostfxr_set_runtime_property_value_fn set_prop_fptr;

void init_dotnet();
bool start_clr();
bool load_hostfxr();
load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t* config_path, const char_t* base_directory);
void* load_library(const char* path);
void* get_export(void* h, const char* name);
