using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tomat.GameMaker.IFF.DataTypes;

internal abstract class AbstractSerializableWithComponents : IGameMakerSerializableWithComponents {
    public Dictionary<Type, object> Components { get; } = new();

    public abstract void Read(DeserializationContext context);

    public abstract void Write(SerializationContext context);

    public bool TryGetComponent<T>([NotNullWhen(returnValue: true)] out T? component) where T : class {
        Components.TryGetValue(typeof(T), out var value);
        component = value as T;
        return component != null;
    }

    public void AddComponent<T>(T component) where T : class {
        AddComponent(typeof(T), component);
    }

    public void AddComponent(Type type, object component) {
        Components.Add(type, component);
    }
}
