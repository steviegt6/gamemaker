using Newtonsoft.Json;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.AGRP;

namespace Tomat.GameMaker.AssetDumper.Chunks.AGRP;

public sealed class AgrpChunkDumper : AbstractChunkDumper<IAgrpChunk> {
    public override void DumpChunk(DeserializationContext context, IAgrpChunk chunk, IGameMakerDecompiler? decompiler, string directory) {
        base.DumpChunk(context, chunk, decompiler, directory);

        var audioGroupNames = chunk.AudioGroups.Select(x => x.ExpectObject().Name.ExpectObject().Value).ToArray();
        var audioGroupNamesJson = JsonConvert.SerializeObject(audioGroupNames, Formatting.Indented);
        File.WriteAllText(Path.Combine(directory, "audio_groups.json"), audioGroupNamesJson);
    }
}
