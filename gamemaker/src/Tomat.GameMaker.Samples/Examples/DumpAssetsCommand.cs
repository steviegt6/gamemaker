using System.IO;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Tomat.GameMaker.AssetDumper;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks.GEN8;

namespace Tomat.GameMaker.Samples.Examples;

[Command("dump-assets")]
public sealed class DumpAssetsCommand : BaseCommand {
    [CommandOption("output-path", 'o', Description = "The path to the directory to dump assets to.")]
    public string? OutputPath { get; set; }

    public override ValueTask ExecuteAsync(IConsole console) {
        ValidateWadExists();

        using var stream = File.Open(WadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var wad = GameMakerIffFile.FromStream(stream, out var ctx);
        OutputPath ??= wad.GetChunk<IGen8Chunk>().DisplayName.TryGetObject(out var displayName) ? displayName.Value : Path.GetFileNameWithoutExtension(WadPath);
        ctx.DumpAssets(OutputPath, null);

        return default;
    }
}
