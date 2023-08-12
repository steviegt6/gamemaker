using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Language;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.LANG;

internal sealed class GameMakerLangChunk : AbstractChunk,
                                           ILangChunk {
    public const string NAME = "LANG";

    public int UnknownInt32 { get; set; }

    public int LanguageCount { get; set; }

    public int EntryCount { get; set; }

    public List<GameMakerPointer<GameMakerString>> EntryIds { get; set; } = null!;

    public List<GameMakerLanguage> Languages { get; set; } = null!;

    public GameMakerLangChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        UnknownInt32 = context.ReadInt32();
        LanguageCount = context.ReadInt32();
        EntryCount = context.ReadInt32();

        EntryIds = new List<GameMakerPointer<GameMakerString>>();
        for (var i = 0; i < EntryCount; i++)
            EntryIds.Add(context.ReadPointerAndObject<GameMakerString>());

        Languages = new List<GameMakerLanguage>();

        for (var i = 0; i < LanguageCount; i++) {
            var language = new GameMakerLanguage(EntryCount);
            language.Read(context);
            Languages.Add(language);
        }
    }

    public override void Write(SerializationContext context) {
        context.Write(UnknownInt32);
        LanguageCount = Languages.Count;
        context.Write(LanguageCount);
        EntryCount = EntryIds.Count;
        context.Write(EntryCount);

        foreach (var entry in EntryIds)
            context.Write(entry);

        foreach (var language in Languages)
            language.Write(context);
    }
}
