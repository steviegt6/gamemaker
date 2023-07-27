using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.AssetDumper;

public static class AssetDumperExtensions {
    public static void DumpAssets(this DeserializationContext context, string outputPath, IGameMakerDecompiler? decompiler) {
        if (Directory.Exists(outputPath))
            Directory.Delete(outputPath, true);

        Directory.CreateDirectory(outputPath);
        var assetDumper = new GameMakerIffAssetDumper(context, decompiler);
        assetDumper.DumpTo(outputPath);
    }
}
