﻿using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Language;

public sealed class GameMakerLanguage : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Region { get; set; }

    public List<GameMakerPointer<GameMakerString>>? Entries { get; set; }

    private readonly int entryCount;

    public GameMakerLanguage(int entryCount) {
        this.entryCount = entryCount;
    }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Region = context.ReadPointerAndObject<GameMakerString>();

        Entries = new List<GameMakerPointer<GameMakerString>>();
        for (var i = 0; i < entryCount; i++)
            Entries.Add(context.ReadPointerAndObject<GameMakerString>());
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Region);

        foreach (var entry in Entries!)
            context.Write(entry);
    }
}
