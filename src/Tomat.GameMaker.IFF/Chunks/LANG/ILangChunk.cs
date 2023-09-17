using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Language;

namespace Tomat.GameMaker.IFF.Chunks.LANG;

public interface ILangChunk : IGameMakerChunk {
    int UnknownInt32 { get; set; }

    int LanguageCount { get; set; }

    int EntryCount { get; set; }

    List<GameMakerPointer<GameMakerString>> EntryIds { get; set; }

    List<GameMakerLanguage> Languages { get; set; }
}
