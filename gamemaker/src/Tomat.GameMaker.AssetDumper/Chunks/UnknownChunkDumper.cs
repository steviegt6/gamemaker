using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.AssetDumper.Chunks;

public sealed class UnknownChunkDumper : AbstractChunkDumper<IGameMakerChunk> {
    public override void DumpChunk(DeserializationContext context, IGameMakerChunk chunk, IGameMakerDecompiler? decompiler, string directory) {
        base.DumpChunk(context, chunk, decompiler, directory);

        var chunkData = context.Data[chunk.StartPosition..chunk.EndPosition];
        File.WriteAllBytes(Path.Combine(directory, $"{chunk.Name}.chnk"), chunkData);
    }
}
