#include <cstdio>
#include <string>
#include <windows.h>
#include "log.h"

// https://github.com/Archie-osu/YYToolkit/blob/stable/YYToolkit/Src/Core/Utils/Logging/Logging.cpp

void set_console_color(const console_color color)
{
    const HANDLE console = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(console, static_cast<WORD>(color));
}

void msg(console_color color, const char* format, ...)
{
    constexpr static int max_buffer_size = 1024;

    va_list args;
    va_start(args, format);

    if (strlen(format) >= max_buffer_size)
    {
        msg(light_red, "Attempted to log too long of a message!");
        return;
    }

    char buffer[max_buffer_size] = {};
    strncpy_s(buffer, format, max_buffer_size);
    (void)vsprintf_s(buffer, format, args);
    const std::string message(buffer);

    va_end(args);

    set_console_color(color);
    printf("%s", message.c_str());
    set_console_color(light_gray);
}

// void err(console_color color, const char* format, ...);
