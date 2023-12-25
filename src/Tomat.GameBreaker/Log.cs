using System;
using Tomat.GameBreaker.Framework.Logging;

namespace Tomat.GameBreaker;

internal static class Log {
    private static Logger logger = null!;

    public static void Init(Logger instance) {
        logger = instance;
    }

    public static void Trace(string message) {
        logger.Trace(message);
    }

    public static void Debug(string message) {
        logger.Debug(message);
    }

    public static void Info(string message) {
        logger.Info(message);
    }

    public static void Warn(string message) {
        logger.Warn(message);
    }

    public static void Error(string message) {
        logger.Error(message);
    }

    public static void Fatal(string message) {
        logger.Fatal(message);
    }

    public static Logger AsType<T>() {
        return AsType(typeof(T));
    }

    public static Logger AsType(Type type) {
        return logger.MakeChildFromType(type);
    }
}
