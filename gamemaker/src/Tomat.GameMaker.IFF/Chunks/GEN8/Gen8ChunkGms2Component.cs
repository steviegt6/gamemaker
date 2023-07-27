using System;
using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.GEN8;

public interface IGen8ChunkGms2Component {
    List<long> RandomUid { get; set; }

    float Fps { get; set; }

    bool AllowStatistics { get; set; }

    Guid GameGuid { get; set; }
}

internal sealed class Gen8ChunkGms2Component : IGen8ChunkGms2Component {
    public required List<long> RandomUid { get; set; }

    public required float Fps { get; set; }

    public required bool AllowStatistics { get; set; }

    public required Guid GameGuid { get; set; }
}
