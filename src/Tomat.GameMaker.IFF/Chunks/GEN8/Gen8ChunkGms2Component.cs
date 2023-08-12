using System;
using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GEN8;

/// <summary>
///     Contains <c>GEN8</c> information only included in GameMaker: Studio,
///     version 2 or later games.
/// </summary>
public interface IGen8ChunkGms2Component {
    /// <summary>
    ///     Integrity-related checksum.
    /// </summary>
    List<long> RandomUid { get; set; }

    /// <summary>
    ///     The FPS of the game.
    /// </summary>
    float Fps { get; set; }

    /// <summary>
    ///     Whether to allow statistics.
    /// </summary>
    bool AllowStatistics { get; set; }

    /// <summary>
    ///     Integrity-related checksum data.
    /// </summary>
    Guid GameGuid { get; set; }
}

internal sealed class Gen8ChunkGms2Component : IGen8ChunkGms2Component {
    public required List<long> RandomUid { get; set; }

    public required float Fps { get; set; }

    public required bool AllowStatistics { get; set; }

    public required Guid GameGuid { get; set; }
}
