using Tomat.GameMaker.AssetDumper.Chunks;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.AssetDumper;

public sealed class GameMakerIffAssetDumper {
    private readonly DeserializationContext context;
    private readonly IGameMakerDecompiler? decompiler;

    public GameMakerIffAssetDumper(DeserializationContext context, IGameMakerDecompiler? decompiler) {
        this.context = context;
        this.decompiler = decompiler;
    }

    public void DumpTo(string directory) {
        Directory.CreateDirectory(directory);

        new UnknownChunkDumper().DumpChunk(context, context.IffFile.Form, decompiler, Path.Combine(directory, "FORM"));

        foreach (var chunk in context.IffFile.Form.Chunks) {
            switch (chunk.Value) {
                default:
                    new UnknownChunkDumper().DumpChunk(context, chunk.Value, decompiler, Path.Combine(directory, chunk.Key));
                    break;
            }
        }
    }
}
