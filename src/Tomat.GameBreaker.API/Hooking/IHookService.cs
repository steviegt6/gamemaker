using System;

namespace Tomat.GameBreaker.API.Hooking;

/// <summary>
///     A detouring/hooking service for native functions in a managed context.
/// </summary>
public interface IHookService {
    /// <summary>
    ///     Creates a hook for a native function.
    /// </summary>
    /// <param name="moduleName">
    ///     The name of the module that contains the function.
    /// </param>
    /// <param name="functionName">The name of the function to hook.</param>
    /// <param name="callback">
    ///     The callback to invoke when the function is called.
    /// </param>
    /// <typeparam name="TDelegate">
    ///     The managed native function signature.
    /// </typeparam>
    /// <returns>The original native function.</returns>
    TDelegate CreateHook<TDelegate>(string moduleName, string functionName, TDelegate callback) where TDelegate : Delegate;

    /// <summary>
    ///     Creates a hook for a native function.
    /// </summary>
    /// <param name="address">The address of the function to hook.</param>
    /// <param name="callback">
    ///     The callback to invoke when the function is called.
    /// </param>
    /// <typeparam name="TDelegate">
    ///     The managed native function signature.
    /// </typeparam>
    /// <returns>The original native function.</returns>
    TDelegate CreateHook<TDelegate>(nint address, TDelegate callback) where TDelegate : Delegate;
}
