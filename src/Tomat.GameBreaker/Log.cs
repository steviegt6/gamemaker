using System;
using Tomat.GameBreaker.Logging;

namespace Tomat.GameBreaker;

internal static class Log {
    internal static Logger Logger = null!;

    public static Logger GetLogger(Type type) {
        return Logger.MakeChildFromType(type);
    }
}
