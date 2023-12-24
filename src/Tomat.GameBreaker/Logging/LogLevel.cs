using System;

namespace Tomat.GameBreaker.Logging;

public interface ILogLevel {
    string Name { get; }

    float Severity { get; }

    ConsoleColor Color { get; }
}

public readonly record struct LogLevel(string Name, float Severity, ConsoleColor Color) : ILogLevel;

public static class LogLevels {
    public static readonly ILogLevel TRACE = new LogLevel("Trace", 0.0f, ConsoleColor.DarkGray);
    public static readonly ILogLevel DEBUG = new LogLevel("Debug", 1.0f, ConsoleColor.Gray);
    public static readonly ILogLevel INFO = new LogLevel("Info", 2.0f, ConsoleColor.White);
    public static readonly ILogLevel WARN = new LogLevel("Warn", 3.0f, ConsoleColor.Yellow);
    public static readonly ILogLevel ERROR = new LogLevel("Error", 4.0f, ConsoleColor.Red);
    public static readonly ILogLevel FATAL = new LogLevel("Fatal", 5.0f, ConsoleColor.DarkRed);
}
