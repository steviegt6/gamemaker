#include <string>
#include <windows.h>
#include <winternl.h>

#include "api.h"
#include "config.h"
#include "console.h"
#include "dotnet.h"
#include "log.h"

// Proxying technique taken from Archie's Uniprox project.
// https://github.com/Archie-osu/Uniprox
// GNU General Public License, version 3

void load_uniprox_dlls(const std::wstring& cwd)
{
    // msg(light_gray, "Loading Uniprox DLLs...\n");

    const std::wstring uniprox_dir_path = cwd + L"\\uniprox";

    msg(light_gray, "Loading Uniprox DLLs from %S...\n", uniprox_dir_path.c_str());

    for (std::error_code error; const auto& file : std::filesystem::directory_iterator(uniprox_dir_path, error))
    {
        if (error)
        {
            msg(light_red, "Failed to iterate directory %s with error code %d.\n", uniprox_dir_path.c_str(), error.value());
            return;
        }

        if (!file.is_regular_file(error))
            continue;

        if (!file.path().has_filename())
            continue;

        if (!file.path().filename().has_extension())
            continue;

        if (!file.path().filename().extension().compare(".dll"))
        {
            msg(light_gray, "Loading %S...\n", file.path().c_str());
            if (const HMODULE module = LoadLibrary(file.path().c_str()); module == nullptr)
            {
                msg(light_red, "Failed to load %s with error code %d.\n", file.path().c_str(), GetLastError());
                return;
            }
        }
    }
}

void suspend_process(const DWORD process_id)
{
    const HANDLE process = OpenProcess(PROCESS_ALL_ACCESS, FALSE, process_id);
    if (process == nullptr)
    {
        msg(light_red, "Failed to open process %d with error code %d.\n", process_id, GetLastError());
        return;
    }

    SuspendThread(process);
    CloseHandle(process);
}

typedef enum _KWAIT_REASON
{
    Executive = 0,
    FreePage = 1,
    PageIn = 2,
    PoolAllocation = 3,
    DelayExecution = 4,
    Suspended = 5,
    UserRequest = 6,
    WrExecutive = 7,
    WrFreePage = 8,
    WrPageIn = 9,
    WrPoolAllocation = 10,
    WrDelayExecution = 11,
    WrSuspended = 12,
    WrUserRequest = 13,
    WrEventPair = 14,
    WrQueue = 15,
    WrLpcReceive = 16,
    WrLpcReply = 17,
    WrVirtualMemory = 18,
    WrPageOut = 19,
    WrRendezvous = 20,
    Spare2 = 21,
    Spare3 = 22,
    Spare4 = 23,
    Spare5 = 24,
    WrCalloutStack = 25,
    WrKernel = 26,
    WrResource = 27,
    WrPushLock = 28,
    WrMutex = 29,
    WrQuantumEnd = 30,
    WrDispatchInt = 31,
    WrPreempted = 32,
    WrYieldExecution = 33,
    WrFastMutex = 34,
    WrGuardedMutex = 35,
    WrRundown = 36,
    MaximumWaitReason = 37
} KWAIT_REASON;

typedef enum _KTHREAD_STATE
{
    Initialized,
    Ready,
    Running,
    Standby,
    Terminated,
    Waiting,
    Transition,
    DeferredReady,
    GateWaitObsolete,
    WaitingForProcessInSwap,
    MaximumThreadState
} KTHREAD_STATE, *PKTHREAD_STATE;

typedef struct _SYSTEM_THREAD
{
    LARGE_INTEGER KernelTime;
    LARGE_INTEGER UserTime;
    LARGE_INTEGER CreateTime;
    ULONG WaitTime;
    PVOID StartAddress;
    CLIENT_ID ClientId;
    KPRIORITY Priority;
    LONG BasePriority;
    ULONG ContextSwitchCount;
    KTHREAD_STATE State;
    KWAIT_REASON WaitReason;
} SYSTEM_THREAD, *PSYSTEM_THREAD;

typedef struct _VM_COUNTERS
{
    SIZE_T PeakVirtualSize;
    SIZE_T VirtualSize;
    ULONG PageFaultCount;
    SIZE_T PeakWorkingSetSize;
    SIZE_T WorkingSetSize;
    SIZE_T QuotaPeakPagedPoolUsage;
    SIZE_T QuotaPagedPoolUsage;
    SIZE_T QuotaPeakNonPagedPoolUsage;
    SIZE_T QuotaNonPagedPoolUsage;
    SIZE_T PagefileUsage;
    SIZE_T PeakPagefileUsage;
} VM_COUNTERS, *PVM_COUNTERS;

typedef struct system_process_information
{
    // NOLINT(clang-diagnostic-zero-length-array)
    ULONG NextEntryOffset;
    ULONG NumberOfThreads;
    LARGE_INTEGER Reserved[3];
    LARGE_INTEGER CreateTime;
    LARGE_INTEGER UserTime;
    LARGE_INTEGER KernelTime;
    UNICODE_STRING ImageName;
    KPRIORITY BasePriority;
    HANDLE ProcessId;
    HANDLE InheritedFromProcessId;
    ULONG HandleCount;
    ULONG Reserved2[2];
    ULONG PrivatePageCount;
    VM_COUNTERS VirtualMemoryCounters;
    IO_COUNTERS IoCounters;
    SYSTEM_THREAD Threads[0];
} system_process_information, *system_process_information_ptr;

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

using process_iterator_func = void(*)(system_process_information* process_info, void* parameter);

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

bool is_suspended()
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
            HANDLE thread = OpenThread(THREAD_ALL_ACCESS, false, reinterpret_cast<uintptr_t>(process_info->Threads[i].ClientId.UniqueThread)); // NOLINT(performance-no-int-to-ptr, clang-diagnostic-shorten-64-to-32)

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

// We need to inject manually since dbgcore.dll won't load in a suspended process.
void inject_into_process(const PROCESS_INFORMATION proc_info)
{
    const auto dll_path = "dbgcore.dll";

    const HMODULE kernel32 = GetModuleHandleA("kernel32.dll");
    const FARPROC load_library = GetProcAddress(kernel32, "LoadLibraryA");

    const LPVOID alloc_path = VirtualAllocEx(proc_info.hProcess, nullptr, strlen(dll_path) + 1, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

    WriteProcessMemory(proc_info.hProcess, alloc_path, dll_path, strlen(dll_path) + 1, nullptr);
    CreateRemoteThread(proc_info.hProcess, nullptr, 0, reinterpret_cast<LPTHREAD_START_ROUTINE>(load_library), alloc_path, 0, nullptr);
}

void restart_as_suspended()
{
    const LPCSTR command_line_c = GetCommandLineA();
    char command_line[MAX_PATH];
    strcpy_s(command_line, sizeof command_line, command_line_c);
    strcat_s(command_line, sizeof command_line, " -suspended");

    char current_directory[MAX_PATH];
    GetCurrentDirectoryA(MAX_PATH, current_directory);

    PROCESS_INFORMATION process_information;
    STARTUPINFOA startup_info;
    ZeroMemory(&process_information, sizeof(PROCESS_INFORMATION));
    ZeroMemory(&startup_info, sizeof(STARTUPINFOA));
    startup_info.cb = sizeof(STARTUPINFOA);
    if (!CreateProcessA(nullptr, command_line, nullptr, nullptr, false, CREATE_SUSPENDED, nullptr, current_directory, &startup_info, &process_information))
    {
        // const auto error = GetLastError();
        throw std::exception("Failed to create process.");
    }
    inject_into_process(process_information);
    TerminateProcess(GetCurrentProcess(), 0);
}

DWORD thread_main(LPVOID)
{
    if (!is_suspended())
    {
        restart_as_suspended();
        return true;
    }

    TCHAR cwd_buf[MAX_PATH];
    GetModuleFileName(nullptr, cwd_buf, MAX_PATH);
    std::wstring cwd(cwd_buf);
    const std::wstring::size_type separator = cwd.find_last_of(L"\\/");
    cwd = cwd.substr(0, separator);

    init_console();

    size_t required_size;
    _wgetenv_s(&required_size, nullptr, 0, L"APPDATA");
    std::wstring appdata_dir(required_size, L'\0');
    _wgetenv_s(&required_size, appdata_dir.data(), required_size, L"APPDATA");
    appdata_dir.resize(required_size - 1); // Remove null terminator.
    const std::wstring gamebreaker_dir = appdata_dir + L"\\gamebreaker";
    const std::wstring managed_host_dir = gamebreaker_dir + L"\\managed_host";

    const nlohmann::json* p_json = init_config(cwd, managed_host_dir);
    if (!p_json)
    {
        MessageBox(nullptr, L"Failed to initialize config, cancelling injection.", L"Tomat.GameBreaker.Host", MB_OK | MB_ICONERROR);
        return 0;
    }

    const nlohmann::json json = *p_json;

    if (!init_dotnet(json, cwd, managed_host_dir))
    {
        MessageBox(nullptr, L"Failed to initialize .NET, cancelling injection.", L"Tomat.GameBreaker.Host", MB_OK | MB_ICONERROR);
        return 0;
    }

    if (json["actAsUniprox"].get<bool>())
        load_uniprox_dlls(cwd);

    delete p_json;

    while (true)
        Sleep(1000);

    // return 0;
}

// ReSharper disable once CppInconsistentNaming
BOOL APIENTRY DllMain(const HMODULE instance, const DWORD reason, LPVOID)
{
    if (reason != DLL_PROCESS_ATTACH)
        return true;

    CloseHandle(CreateThread(nullptr, 0, thread_main, instance, 0, nullptr));
    return true;
}

/*extern "C" __declspec(dllexport) void resume_process(void)
{
    process_iterator_func func = [](system_process_information* proc_info, void*)
    {
        const LPMODULEINFO game_module{};
        get_module_information(nullptr, game_module);

        if (proc_info == nullptr)
            return;

        if (reinterpret_cast<uintptr_t>(proc_info->ProcessId) != GetCurrentProcessId())
            return;

        for (int i = 0; i < proc_info->NumberOfThreads; i++)
        {
            const HANDLE thread = OpenThread(THREAD_ALL_ACCESS, false, reinterpret_cast<uintptr_t>(proc_info->Threads[i].ClientId.UniqueThread)); // NOLINT(clang-diagnostic-shorten-64-to-32)

            unsigned long start_addr;
            get_thread_start_address(thread, start_addr);

            if (start_addr == reinterpret_cast<uintptr_t>(game_module->EntryPoint))
                ResumeThread(thread);

            CloseHandle(thread);
        }
    };

    iterate_processes(func, nullptr);
}*/

extern "C" __declspec(dllexport) BOOL MiniDumpReadDumpStream(PVOID, ULONG, void*, void*, void*)
{
    return false;
}

extern "C" __declspec(dllexport) BOOL MiniDumpWriteDump(HANDLE, DWORD, HANDLE, DWORD, void*, void*, void*)
{
    return false;
}
