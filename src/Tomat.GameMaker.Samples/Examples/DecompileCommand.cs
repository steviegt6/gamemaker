/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.Decompiler.Disassembler;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks.CODE;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;

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
        // var decompiler = new DogScepterDisassemblerGameMakerDecompiler();
        var decompiler = new UndertaleModToolDisassemblerGameMakerDecompiler();
        var decompilerContext = new DecompilerContext(ctx, decompiler);

        var codeChunk = wad.GetChunk<ICodeChunk>();
        if (!codeChunk.TryGetComponent<ICodeChunkCodeComponent>(out var codeComponent))
            throw new Exception("CODE chunk does not have a code component!");

        var outDir = Path.Combine(Path.GetDirectoryName(WadPath)!, Path.GetFileNameWithoutExtension(WadPath) + "_decompiled");
        var globalDir = Path.Combine(outDir, "global");
        var roomDir = Path.Combine(outDir, "room");
        var roomCcDir = Path.Combine(outDir, "roomcc");
        var objectDir = Path.Combine(outDir, "object");
        Directory.CreateDirectory(outDir);
        Directory.CreateDirectory(globalDir);
        Directory.CreateDirectory(roomDir);
        Directory.CreateDirectory(roomCcDir);
        Directory.CreateDirectory(objectDir);

        if (CodeName is null) {
            var globalScripts = new List<GameMakerCode>();
            var roomScripts = new List<GameMakerCode>();
            var roomCcScripts = new List<GameMakerCode>();
            var objectScripts = new List<GameMakerCode>();
            var scripts = new List<GameMakerCode>();

            foreach (var code in codeComponent.Code!.Select(x => x.ExpectObject())) {
                var name = code.Name.ExpectObject().Value!;

                if (name.StartsWith("gml_GlobalScript_"))
                    globalScripts.Add(code);
                else if (name.StartsWith("gml_Object_"))
                    objectScripts.Add(code);
                else if (name.StartsWith("gml_Room_"))
                    roomScripts.Add(code);
                else if (name.StartsWith("gml_RoomCC_"))
                    roomCcScripts.Add(code);
                else if (name.StartsWith("gml_Script_"))
                    scripts.Add(code);
                else
                    throw new Exception($"Unknown code name: {name}");
            }

            void logDecompilation(string name, DecompilerResult result) {
                console.Output.WriteLine($"Decompiled function '{result}' with {result.Warnings.Count} warning(s) and {result.Errors.Count} error(s)");

                if (result.Warnings.Count > 0) {
                    foreach (var warning in result.Warnings)
                        console.Output.WriteLine($"  Warning: {warning}");
                }

                if (result.Errors.Count > 0) {
                    foreach (var error in result.Errors)
                        console.Output.WriteLine($"  Error: {error}");
                }
            }

            foreach (var globalScript in globalScripts) {
                var result = decompiler.DecompileFunction(decompilerContext, globalScript);
                var codeName = globalScript.Name!.ExpectObject().Value!;
                logDecompilation(codeName, result);
                var codePath = Path.Combine(globalDir, codeName + ".txt");
                File.WriteAllText(codePath, result.GetCode());
            }

            foreach (var roomScript in roomScripts) {
                var result = decompiler.DecompileFunction(decompilerContext, roomScript);
                var codeName = roomScript.Name!.ExpectObject().Value!;
                logDecompilation(codeName, result);
                var codePath = Path.Combine(roomDir, codeName + ".txt");
                File.WriteAllText(codePath, result.GetCode());
            }

            foreach (var roomCcScript in roomCcScripts) {
                var result = decompiler.DecompileFunction(decompilerContext, roomCcScript);
                var codeName = roomCcScript.Name!.ExpectObject().Value!;
                logDecompilation(codeName, result);
                var codePath = Path.Combine(roomCcDir, codeName + ".txt");
                File.WriteAllText(codePath, result.GetCode());
            }

            foreach (var objectScript in objectScripts) {
                var result = decompiler.DecompileFunction(decompilerContext, objectScript);
                var codeName = objectScript.Name!.ExpectObject().Value!;
                logDecompilation(codeName, result);
                var codePath = Path.Combine(objectDir, codeName + ".txt");
                File.WriteAllText(codePath, result.GetCode());
            }

            // No need to handle `gml_Script_`s since they're children of
            // GlobalScripts and Objects.
        }
        else {
            var code = codeComponent.Code!.FirstOrDefault(c => c.ExpectObject().Name!.ExpectObject().Value == CodeName);

            if (!code.IsNull) {
                var codeObj = code.ExpectObject();
                var result = decompiler.DecompileFunction(decompilerContext, codeObj);
                var codePath = Path.Combine(outDir, CodeName);
                // limit to windows path length + .txt
                if (codePath.Length > 260 - 4)
                    codePath = codePath[..(260 - 4)];
                codePath += ".txt";
                File.WriteAllText(codePath, result.GetCode());
            }
            else {
                console.Output.WriteLine($"Code entry '{CodeName}' not found!");
            }
        }

        return default;
    }
}
*/