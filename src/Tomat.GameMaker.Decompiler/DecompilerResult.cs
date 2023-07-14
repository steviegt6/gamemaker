using System.Collections.Generic;

namespace Tomat.GameMaker.Decompiler;

public record DecompilerResult(string? DecompiledCode, List<string> Warnings, List<string> Errors);
