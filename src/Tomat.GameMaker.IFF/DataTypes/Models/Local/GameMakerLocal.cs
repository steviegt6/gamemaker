﻿using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Local;

public sealed class GameMakerLocal : IGameMakerSerializable {
    public int Index { get; set; }

    public GameMakerPointer<GameMakerString> Name { get; set; }

    public void Read(DeserializationContext context) {
        Index = context.Reader.ReadInt32();
        Name = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Write(Index);
        context.Write(Name);
    }
}