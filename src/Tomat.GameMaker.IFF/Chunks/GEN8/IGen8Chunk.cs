using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.GEN8;

/// <summary>
///     The <c>GEN8</c> chunk, which contains general information.
/// </summary>
/// <remarks>
///     If the bytecode version is 14 or greater,
///     <see cref="IGen8ChunkDebuggerPortComponent"/> is included.
///     <br />
///     if the IDE version is 2.0.0 or greater,
///     <see cref="IGen8ChunkGms2Component"/> is included.
/// </remarks>
/// <seealso cref="IGen8ChunkDebuggerPortComponent"/>
/// <seealso cref="IGen8ChunkGms2Component"/>
public interface IGen8Chunk : IGameMakerChunk {
    /// <summary>
    ///     Whether debugging support is disabled.
    /// </summary>
    bool DisableDebug { get; set; }

    /// <summary>
    ///     The bytecode version.
    /// </summary>
    byte FormatId { get; set; }

    /// <summary>
    ///     An unknown value.
    /// </summary>
    short UnknownInt16 { get; set; }

    /// <summary>
    ///     The file name of the runner.
    /// </summary>
    GameMakerPointer<GameMakerString> FileName { get; set; }

    /// <summary>
    ///     The GameMaker configuration file that this data file was compiled
    ///     with.
    /// </summary>
    GameMakerPointer<GameMakerString> Config { get; set; }

    /// <summary>
    ///     The last object ID of the data file.
    /// </summary>
    int LastObjectId { get; set; }

    /// <summary>
    ///     The last tile ID of the data file.
    /// </summary>
    int LastTileId { get; set; }

    /// <summary>
    ///     The Game ID of the data file.
    /// </summary>
    int GameId { get; set; }

    /// <summary>
    ///     A legacy GUID representing the DirectPlay GUID, which is always
    ///     empty in GameMaker: Studio, version 2 or later.
    /// </summary>
    Guid LegacyGuid { get; set; }

    /// <summary>
    ///     The name of the game.
    /// </summary>
    GameMakerPointer<GameMakerString> GameName { get; set; }

    /// <summary>
    ///     The major version of the IDE used to build the game.
    /// </summary>
    /// <remarks>This is rarely updated, if ever.</remarks>
    int MajorVersion { get; set; }

    /// <summary>
    ///     The minor version of the IDE used to build the game.
    /// </summary>
    /// <remarks>This is rarely updated, if ever.</remarks>
    int MinorVersion { get; set; }

    /// <summary>
    ///     The release version of the IDE used to build the game.
    /// </summary>
    /// <remarks>This is rarely updated, if ever.</remarks>
    int ReleaseVersion { get; set; }

    /// <summary>
    ///     The build version of the IDE used to build the game.
    /// </summary>
    /// <remarks>This is rarely updated, if ever.</remarks>
    int BuildVersion { get; set; }

    /// <summary>
    ///     The default window width of the game.
    /// </summary>
    int DefaultWindowWidth { get; set; }

    /// <summary>
    ///     The default window height of the game.
    /// </summary>
    int DefaultWindowHeight { get; set; }

    /// <summary>
    ///     Various informational flags.
    /// </summary>
    Gen8InfoFlags Info { get; set; }

    /// <summary>
    ///     The CRC32 of the license used to compile the game.
    /// </summary>
    uint LicenseCrc32 { get; set; }

    /// <summary>
    ///     The MD5 of the license used to compile the game.
    /// </summary>
    Memory<byte> LicenseMd5 { get; set; }

    /// <summary>
    ///     The UNIX timestamp of when the game was compiled.
    /// </summary>
    long Timestamp { get; set; }

    /// <summary>
    ///     The name that gets displayed in the game window.
    /// </summary>
    GameMakerPointer<GameMakerString> DisplayName { get; set; }

    /// <summary>
    ///     
    /// </summary>
    long ActiveTargets { get; set; }

    /// <summary>
    ///     The function classifications of this data file.
    /// </summary>
    Gen8FunctionClassification FunctionClassifications { get; set; }

    /// <summary>
    ///     The Steam App ID of the game.
    /// </summary>
    int SteamAppId { get; set; }

    /// <summary>
    ///     The room order of the data file.
    /// </summary>
    List<int> RoomOrder { get; set; }
}
