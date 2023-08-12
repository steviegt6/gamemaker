using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tomat.GameMaker.IFF.Chunks;

public interface IGameMakerSerializableWithComponents : IGameMakerSerializable {
    Dictionary<Type, object> Components { get; }

    bool TryGetComponent<T>([NotNullWhen(returnValue: true)] out T? component) where T : class;

    void AddComponent<T>(T component) where T : class;

    void AddComponent(Type type, object component);
}
