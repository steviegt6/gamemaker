#include "api.h"

extern "C" __declspec(dllexport) BOOL find_byte_array(const byte* pattern, const int pattern_length, const byte* mask, const int mask_length, uintptr_t region_base, uintptr_t region_size, uintptr_t& result)
{
    result = 0;

    if (!region_base && !region_size)
    {
        MODULEINFO module_info;
        if (!get_module_information(nullptr, &module_info))
            return false;

        region_base = reinterpret_cast<uintptr_t>(module_info.lpBaseOfDll);
        region_size = module_info.SizeOfImage;
    }

    const size_t pattern_size = pattern_length < mask_length ? pattern_length : mask_length;
    for (unsigned i = 0; i < region_size - pattern_size; i++)
    {
        bool found = true;
        for (unsigned j = 0; j < pattern_size; j++)
            found &= mask[j] == '?' || pattern[j] == *reinterpret_cast<const unsigned char*>(region_base + i + j); // NOLINT(performance-no-int-to-ptr)

        if (found)
        {
            result = region_base + i; // NOLINT(performance-no-int-to-ptr)
            return true;
        }
    }

    return false;
}

bool get_module_information(const char* module_name, const LPMODULEINFO module_info)
{
    using fn = int(__stdcall*)(HANDLE, HMODULE, LPMODULEINFO, DWORD);

    const HMODULE kernel32_handle = GetModuleHandleA("kernel32.dll");
    const auto get_module_info = reinterpret_cast<fn>(GetProcAddress(kernel32_handle, "K32GetModuleInformation")); // NOLINT(clang-diagnostic-cast-function-type)

    const HMODULE module_handle = GetModuleHandleA(module_name);
    if (module_handle == nullptr)
        return false;

    get_module_info(GetCurrentProcess(), module_handle, module_info, sizeof(MODULEINFO));
    return true;
}
