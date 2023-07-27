using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.GEN8;

public interface IGen8Chunk : IGameMakerChunk {
    bool DisableDebug { get; set; }

    byte FormatId { get; set; }

    short UnknownInt16 { get; set; }

    GameMakerPointer<GameMakerString> FileName { get; set; }

    GameMakerPointer<GameMakerString> Config { get; set; }

    int LastObjectId { get; set; }

    int LastTileId { get; set; }

    int GameId { get; set; }

    Guid LegacyGuid { get; set; }

    GameMakerPointer<GameMakerString> GameName { get; set; }

    int MajorVersion { get; set; }

    int MinorVersion { get; set; }

    int ReleaseVersion { get; set; }

    int BuildVersion { get; set; }

    int DefaultWindowWidth { get; set; }

    int DefaultWindowHeight { get; set; }

    Gen8InfoFlags Info { get; set; }

    int LicenseCrc32 { get; set; }

    Memory<byte> LicenseMd5 { get; set; }

    long Timestamp { get; set; }

    GameMakerPointer<GameMakerString> DisplayName { get; set; }

    long ActiveTargets { get; set; }

    Gen8FunctionClassification FunctionClassifications { get; set; }

    int SteamAppId { get; set; }

    List<int> RoomOrder { get; set; }
}
