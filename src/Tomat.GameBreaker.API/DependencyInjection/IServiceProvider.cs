using System;
using System.Diagnostics.CodeAnalysis;

namespace Tomat.GameBreaker.API.DependencyInjection;

public interface IServiceProvider {
    bool TryGetService(Type type, [NotNullWhen(returnValue: true)] out object? service);

    void RegisterService(Type type, object service);
}

public static class ServiceProviderExtensions {
    public static bool TryGetService<T>(this IServiceProvider provider, [NotNullWhen(returnValue: true)] out T? service) {
        if (provider.TryGetService(typeof(T), out var serviceObj)) {
            service = (T)serviceObj;
            return true;
        }

        service = default;
        return false;
    }

    public static void RegisterService<T>(this IServiceProvider provider, T service) where T : notnull {
        provider.RegisterService(typeof(T), service);
    }

    public static object ExpectService(this IServiceProvider provider, Type type) {
        if (provider.TryGetService(type, out var service))
            return service;

        throw new InvalidOperationException($"Service of type {type} is not registered.");
    }

    public static T ExpectService<T>(this IServiceProvider provider) where T : notnull {
        if (provider.TryGetService<T>(out var service))
            return service;

        throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
    }
}
