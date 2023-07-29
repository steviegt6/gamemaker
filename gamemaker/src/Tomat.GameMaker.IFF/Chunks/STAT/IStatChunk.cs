using System;
using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.Chunks.STAT; 

public interface IStatChunk {
    // Null-terminated.
    string Name { get; set; }

    Guid Guid { get; set; }
    
    StatProviderLatency Latency { get; set; }
    
    StatProviderPriority Priority { get; set; }
    
    StatProviderEnabledState EnabledState { get; set; }
    
    int PopulationSampleRates { get; set; }
    
    // int EventCount { get; set; }

    List<StatEvent> Events { get; set; }
}
