#pragma once
#include <windows.h>
#include <psapi.h>

// extern "C" __declspec(dllexport) bool find_byte_array(byte* pattern, int pattern_length, byte* mask, int mask_length, uintptr_t region_base, uintptr_t region_size, uintptr_t& result);

bool get_module_information(const char* module_name, LPMODULEINFO module_info);
