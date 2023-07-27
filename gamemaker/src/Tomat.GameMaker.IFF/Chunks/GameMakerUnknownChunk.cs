using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Tomat.GameMaker.IFF.Chunks;

public sealed class GameMakerUnknownChunk : IGameMakerChunk {
    public Dictionary<Type, object> Components { get; } = new();

    public string Name { get; }

    public int Size { get; set; }

    public byte[]? Data { get; set; }

    public GameMakerUnknownChunk(string name, int size) {
        Name = name;
        Size = size;
    }

    public void Read(DeserializationContext context) {
        Data = context.ReadBytes(Size).ToArray();
    }

    public void Write(SerializationContext context) {
        if (Data is null)
            throw new IOException("Cannot write unknown chunk without data.");

        context.Write(Data);
    }

    public bool TryGetComponent<T>([NotNullWhen(returnValue: true)] out T? component) where T : class {
        component = null;
        return false;
    }

    public void AddComponent<T>(T component) where T : class { }

    public void AddComponent(Type type, object component) { }
}
