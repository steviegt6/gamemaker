using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Spectre.Console;

namespace Tomat.GameMaker.Samples;

public abstract class BaseCommand : ICommand {
    [CommandParameter(0, Name = "WAD path", Description = "The path to the WAD file to read.")]
    public string WadPath { get; set; } = null!;

    public abstract ValueTask ExecuteAsync(IConsole console);

    protected void ValidateWadExists() {
        if (!string.IsNullOrEmpty(WadPath) && File.Exists(WadPath))
            return;

        AnsiConsole.Prompt(
            new TextPrompt<string>("Enter valid WAD path:")
                .ValidationErrorMessage("WAD path does not exist!")
                .Validate(File.Exists)
        );
    }
}
