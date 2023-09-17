#include "api.h"

#include <string>
#include <windows.h>
#include <winternl.h>

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

extern "C" __declspec(dllexport) BOOL is_suspended()
{
    // My code is broken and sucks, let's use this hack.
    const auto command_line = GetCommandLineA();
    if (strstr(command_line, "-suspended"))
        return true;

    const process_iterator_func func = [](system_process_information* process_info, void* is_suspended)
    {
        if (process_info == nullptr)
            return;

        const LPMODULEINFO game_module{};
        get_module_information(nullptr, game_module);

        const HANDLE process_id = process_info->ProcessId;
        if (reinterpret_cast<uintptr_t>(process_id) != GetCurrentProcessId())
            return;

        for (int i = 0; i < process_info->NumberOfThreads; i++) // NOLINT(clang-diagnostic-sign-compare)
        {
            const HANDLE thread = OpenThread(THREAD_ALL_ACCESS, false, reinterpret_cast<uintptr_t>(process_info->Threads[i].ClientId.UniqueThread)); // NOLINT(performance-no-int-to-ptr, clang-diagnostic-shorten-64-to-32)

            unsigned long start_addr = 0;
            const bool success = get_thread_start_address(thread, start_addr);

            CloseHandle(thread);

            if (!success)
                continue;

            if (start_addr == reinterpret_cast<uintptr_t>(game_module->EntryPoint))
            {
                if (process_info->Threads[i].State != Waiting)
                    return;

                if (process_info->Threads[i].WaitReason != Suspended)
                    return;

                *static_cast<bool*>(is_suspended) = true;
            }
        }
    };

    bool is_suspended = false;
    iterate_processes(func, &is_suspended);
    return is_suspended;
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

bool get_proc_info(system_process_information** info)
{
    using fn = NTSTATUS(NTAPI*)(SYSTEM_INFORMATION_CLASS info_class, PVOID system_info, ULONG info_length, PULONG return_length);

    const HMODULE ntdll = GetModuleHandleA("ntdll.dll");

    if (ntdll == nullptr)
        return false;

    const auto nt_query_system_information = reinterpret_cast<fn>(GetProcAddress(ntdll, "NtQuerySystemInformation")); // NOLINT(clang-diagnostic-cast-function-type)
    if (!nt_query_system_information)
        return false;

    uint32_t size = sizeof(system_process_information);
    auto process_info = static_cast<system_process_information*>(malloc(size));

    NTSTATUS status;
    while ((status = nt_query_system_information(SystemProcessInformation, process_info, size, nullptr)) == 0xc0000004 /*STATUS_INFO_LENGTH_MISMATCH*/) // NOLINT(clang-diagnostic-sign-compare)
        process_info = static_cast<system_process_information*>(realloc(process_info, size *= 2));

    if (NT_SUCCESS(status))
    {
        *info = process_info;
        return true;
    }

    return false;
}

void iterate_processes(const process_iterator_func func, void* parameter)
{
    system_process_information* process_info = nullptr;
    if (!get_proc_info(&process_info))
        return;

    void* addr_to_free = process_info;

    const LPMODULEINFO game_module{};
    if (!get_module_information(nullptr, game_module))
        goto FREE; // NOLINT(cppcoreguidelines-avoid-goto, hicpp-avoid-goto)

    while (true)
    {
        func(process_info, parameter);
        if (process_info->NextEntryOffset == 0)
            break;

        process_info = reinterpret_cast<system_process_information*>(reinterpret_cast<uintptr_t>(process_info) + process_info->NextEntryOffset); // NOLINT(performance-no-int-to-ptr)
    }

FREE:
    free(addr_to_free);
}

bool get_thread_start_address(HANDLE thread, unsigned long& address)
{
    using fn = NTSTATUS(NTAPI*)(HANDLE thread_handle, THREADINFOCLASS thread_class, PVOID thread_information, ULONG length, PULONG return_length);

    const HMODULE ntdll = GetModuleHandleA("ntdll.dll");
    if (ntdll == nullptr)
        return false;

    const auto nt_query_information_thread = reinterpret_cast<fn>(GetProcAddress(ntdll, "NtQueryInformationThread")); // NOLINT(clang-diagnostic-cast-function-type)
    if (!nt_query_information_thread)
        return false;

    const NTSTATUS status = nt_query_information_thread(thread, static_cast<THREADINFOCLASS>(9), &address, sizeof(unsigned long), nullptr);
    return NT_SUCCESS(status);
}
