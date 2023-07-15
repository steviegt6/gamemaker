using System.Collections.Generic;
using System.Text;

namespace Tomat.GameMaker.Decompiler;

public class DecompilerResult {
    private readonly string commentPrefix;
    private string? decompiledCode;

    public List<string> Warnings { get; } = new();

    public List<string> Errors { get; } = new();

    public DecompilerResult(string commentPrefix) {
        this.commentPrefix = commentPrefix;
    }

    public DecompilerResult WithCode(string code) {
        decompiledCode = code;
        return this;
    }

    public DecompilerResult WithWarning(string warning) {
        Warnings.Add(warning);
        return this;
    }

    public DecompilerResult WithError(string error) {
        Errors.Add(error);
        return this;
    }

    public string GetCode() {
        var sb = new StringBuilder();

        if (Warnings.Count != 0) {
            sb.AppendLine($"{commentPrefix} WARNINGS:");

            foreach (var warning in Warnings)
                sb.AppendLine($"{commentPrefix}   {warning}");

            sb.AppendLine();
        }

        if (Errors.Count != 0) {
            sb.AppendLine($"{commentPrefix} ERRORS:");

            foreach (var error in Errors)
                sb.AppendLine($"{commentPrefix}   {error}");

            sb.AppendLine();
        }

        if (decompiledCode is not null)
            sb.AppendLine(decompiledCode);
        else
            sb.AppendLine($"{commentPrefix} No code was provided (likely due to an error).");
        
        return sb.ToString();
    }
}
