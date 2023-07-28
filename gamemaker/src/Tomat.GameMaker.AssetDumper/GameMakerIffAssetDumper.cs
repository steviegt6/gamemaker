using Tomat.GameMaker.AssetDumper.Chunks;
using Tomat.GameMaker.AssetDumper.Chunks.ACRV;
using Tomat.GameMaker.AssetDumper.Chunks.AGRP;
using Tomat.GameMaker.AssetDumper.Chunks.FORM;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.ACRV;
using Tomat.GameMaker.IFF.Chunks.AGRP;

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

        new FormChunkDumper().DumpChunk(context, context.IffFile.Form, decompiler, directory /*Path.Combine(directory, "FORM")*/);

        foreach (var chunk in context.IffFile.Form.Chunks) {
            var path = Path.Combine(directory, chunk.Key);

            switch (chunk.Value) {
                case IAcrvChunk acrv:
                    new AcrvChunkDumper().DumpChunk(context, acrv, decompiler, path);
                    break;

                case IAgrpChunk agrp:
                    new AgrpChunkDumper().DumpChunk(context, agrp, decompiler, path);
                    break;

                default:
                    new UnknownChunkDumper().DumpChunk(context, chunk.Value, decompiler, path);
                    break;
            }
        }
    }
}
