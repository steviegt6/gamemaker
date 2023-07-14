using System;
using System.IO;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.Decompiler.CSharp;
using Tomat.GameMaker.Decompiler.Disassembler;
using Tomat.GameMaker.IFF;

namespace Tomat.GameMaker.Samples.Examples;

[Command("decompile", Description = "Decompiles a GameMaker IFF file code entry")]
public class DecompileCommand : BaseCommand {
    [CommandOption("code-name", 'c', Description = "The code entry name to decompile", IsRequired = false)]
    public string? CodeName { get; set; } = null;

    public override ValueTask ExecuteAsync(IConsole console) {
        ValidateWadExists();

        using var stream = File.Open(WadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var wad = GameMakerIffFile.FromStream(stream, out var ctx);
        if (wad.Form is null)
            throw new Exception("FORM chunk is null!");

        var form = wad.Form;
        if (form.Chunks is null)
            throw new Exception("FORM chunk's sub-chunks are null!");

        // var decompiler = new CSharpGameMakerDecompiler();
        var decompiler = new DisassemblerGameMakerDecompiler();
        var decompilerContext = new DecompilerContext(ctx, decompiler);

        if (CodeName is null) {
            var files = decompiler.DecompileIffFile(decompilerContext);
        }
        else {
            // TODO
        }

        return default;
    }
}
