using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks.LANG;

public sealed class GameMakerLangChunk : AbstractChunk {
    public const string NAME = "LANG";
    
    public int UnknownInt32 { get; set; }

    public int LanguageCount { get; set; }

    public int EntryCount { get; set; }

    public List<GameMakerPointer<GameMakerString>>? EntryIds { get; set; }

    public List<GameMakerLanguage>? Languages { get; set; }

    public GameMakerLangChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        UnknownInt32 = context.Reader.ReadInt32();
        LanguageCount = context.Reader.ReadInt32();
        EntryCount = context.Reader.ReadInt32();

        EntryIds = new List<GameMakerPointer<GameMakerString>>();
        for (var i = 0; i < EntryCount; i++)
            EntryIds.Add(context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true));

        Languages = new List<GameMakerLanguage>();

        for (var i = 0; i < LanguageCount; i++) {
            var language = new GameMakerLanguage(EntryCount);
            language.Read(context);
            Languages.Add(language);
        }
    }

    public override void Write(SerializationContext context) {
        context.Writer.Write(UnknownInt32);
        LanguageCount = Languages!.Count;
        context.Writer.Write(LanguageCount);
        EntryCount = EntryIds!.Count;
        context.Writer.Write(EntryCount);

        foreach (var entry in EntryIds!)
            context.Writer.Write(entry);

        foreach (var language in Languages!)
            language.Write(context);
    }
}
