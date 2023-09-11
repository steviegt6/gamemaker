using System;
using System.Diagnostics;

namespace Tomat.GameMaker.IFF;

internal static class ExceptionHelper {
    [StackTraceHidden]
    public static T Expect<T>(this T @this, T value, Exception exception) {
        if (@this is null && value is null)
            return @this;

        if (@this is null || !@this.Equals(value))
            throw exception;

        return @this;
    }
}
