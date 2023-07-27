using Newtonsoft.Json;

namespace Tomat.GameMaker.AssetDumper.Chunks;

public readonly record struct DumpedChunkInfo(
    [JsonProperty("chunkName")]
    string ChunkName,
    /*[JsonProperty("offset")]
    int Offset,*/
    [JsonProperty("length")]
    int Length
);
