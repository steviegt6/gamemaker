using System.Collections.Generic;

namespace Tomat.GameMaker.Decompiler;

public class DecompilerResult {
    public string? DecompiledCode { get; set; }

    public List<string> Warnings { get; } = new();

    public List<string> Errors { get; } = new();

    public DecompilerResult WithCode(string code) {
        DecompiledCode = code;
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
}
