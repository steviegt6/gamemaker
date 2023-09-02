using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tomat.GameBreaker.API.DependencyInjection;

public class DefaultServiceProvider : IServiceProvider {
    private readonly IServiceProvider? parent;
    private readonly Dictionary<Type, object> services = new();

    public DefaultServiceProvider() : this(null) { }

    public DefaultServiceProvider(IServiceProvider? parent) {
        this.parent = parent;
    }

    public bool TryGetService(Type type, [NotNullWhen(returnValue: true)] out object? service) {
        if (parent?.TryGetService(type, out service) ?? false)
            return true;

        return services.TryGetValue(type, out service);
    }

    public void RegisterService(Type type, object service) {
        if (parent?.TryGetService(type, out _) ?? false)
            throw new InvalidOperationException($"Service of type {type} is already registered in parent provider.");

        if (services.ContainsKey(type))
            throw new InvalidOperationException($"Service of type {type} is already registered.");

        services.Add(type, service);
    }
}
