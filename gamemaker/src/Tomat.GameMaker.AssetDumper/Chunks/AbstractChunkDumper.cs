using Newtonsoft.Json;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.AssetDumper.Chunks;

public abstract class AbstractChunkDumper<T> where T : IGameMakerChunk {
    public virtual void DumpChunk(DeserializationContext context, T chunk, IGameMakerDecompiler? decompiler, string directory) {
        Directory.CreateDirectory(directory);

        var chunkInfo = new DumpedChunkInfo(chunk.Name, chunk.StartPosition, chunk.Size);
        var chunkInfoJson = JsonConvert.SerializeObject(chunkInfo, Formatting.Indented);
        File.WriteAllText(Path.Combine(directory, "chunk.json"), chunkInfoJson);
    }
}
