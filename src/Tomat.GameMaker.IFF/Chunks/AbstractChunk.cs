﻿using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.Chunks; 

public abstract class AbstractChunk : IGameMakerChunk {
    public string Name { get; }

    public int Size { get; }
    
    protected AbstractChunk(string name, int size) {
        Name = name;
        Size = size;
    }

    public abstract void Read(DeserializationContext context);

    public abstract void Write(SerializationContext context);
}
