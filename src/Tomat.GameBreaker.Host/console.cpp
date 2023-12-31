﻿#include "console.h"

#include <cstdio>
#include <string>
#include <windows.h>

#include "log.h"

void init_console()
{
    AllocConsole();

    FILE* stream;
    (void)freopen_s(&stream, "CONIN$", "r", stdin);
    (void)freopen_s(&stream, "CONOUT$", "w", stdout);
    (void)freopen_s(&stream, "CONOUT$", "w", stderr);

#ifdef WIN32
    std::string os = "Windows";
    std::string arch = "x86";
#ifdef _WIN64
    arch = "x64";
#endif
#else
    std::string os = "Unknown OS";
    std::string arch = "Unknown Architecture";
#endif

    const std::string console_title = std::string("Tomat.GameBreaker") + " - " + os + " " + arch;
    SetConsoleTitleA(console_title.c_str());

    // Disable selection mode.
    // https://github.com/Archie-osu/YYToolkit/blob/stable/YYToolkit/Src/Core/Features/API/Internal.cpp#L117
    const HANDLE input = GetStdHandle(STD_INPUT_HANDLE);
    DWORD mode;
    GetConsoleMode(input, &mode);
    SetConsoleMode(input, ENABLE_EXTENDED_FLAGS | (mode & ~ENABLE_QUICK_EDIT_MODE));

    msg(light_blue, "Tomat.GameBreaker by Tomat & contributors\n");
    msg(light_blue, "This is free software licensed under the GNU General Public License, version 3\n");
    msg(light_blue, "Consider also checking out:\n");
    msg(light_blue, "- Archie-osu's YYToolkit (which this project is based on) [BSD Zero Clause License]: https://github.com/Archie-osu/YYToolkit\n");
    msg(light_blue, "- Archie-osu's Uniprox (which our injection method is based on) [GNU GPL v3]: https://github.com/Archie-osu/Uniprox\n");
    msg(light_blue, "- nlohmann's json (which is the JSON parser we use) [MIT License]: https://github.com/nlohmann/json\n");
}
