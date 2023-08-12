using Newtonsoft.Json;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.FORM;

namespace Tomat.GameMaker.AssetDumper.Chunks.FORM;

public sealed class FormChunkDumper : AbstractChunkDumper<IFormChunk> {
    public override void DumpChunk(DeserializationContext context, IFormChunk chunk, IGameMakerDecompiler? decompiler, string directory) {
        base.DumpChunk(context, chunk, decompiler, directory);

        var dumpedChunkData = chunk.Chunks.ToDictionary(x => x.Key, x => new DumpedChunkInfo(x.Value.Name, x.Value.StartPosition, x.Value.Size));
        var dumpedChunkDataJson = JsonConvert.SerializeObject(dumpedChunkData, Formatting.Indented);
        File.WriteAllText(Path.Combine(directory, "chunks.json"), dumpedChunkDataJson);
    }
}
