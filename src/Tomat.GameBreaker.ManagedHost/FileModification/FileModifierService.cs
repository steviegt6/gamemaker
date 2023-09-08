using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Tomat.GameBreaker.API.FileModification;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameBreaker.ManagedHost.FileModification;

internal sealed class FileModifierService : IFileModifierService {
    private static readonly string[] known_iff_extensions = { ".3ds", ".dat", ".ios", ".psp", ".symbian", ".unx", ".wiiu", ".win", };

    private readonly List<IFileModifier> fileModifiers = new();
    private readonly List<IIffModifier> iffModifiers = new();

    public void AddFileModifier(IFileModifier modifier) {
        fileModifiers.Add(modifier);
    }

    public void AddIffModifier(IIffModifier modifier) {
        iffModifiers.Add(modifier);
    }

    public bool TryModifyFile(string path, FileContext context, [NotNullWhen(returnValue: true)] out string? newPath) {
        newPath = null;

        var tmpFile = GetTmpFileAndCleanUpLeftoverTmpFiles(path);

        if (IsIff(path, context)) {
            var modifiable = false;
            foreach (var iffModifier in iffModifiers)
                modifiable |= iffModifier.CanModify(path, context);

            if (modifiable) {
                DeserializationContext? deserCtx = null;

                try {
                    using var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    GameMakerIffFile.FromStream(fs, out deserCtx);
                }
                catch (Exception e) {
                    Console.WriteLine($"Failed to read IFF file {path}: {e}");
                }

                if (deserCtx is null)
                    goto TreatAsRegular;

                var modified = false;
                foreach (var iffModifier in iffModifiers)
                    modified |= iffModifier.ModifyIff(path, context, deserCtx);

                if (modified) {
                    using var fs = File.Open(tmpFile, FileMode.Create, FileAccess.Write, FileShare.None);
                    var writer = new GameMakerIffWriter();
                    deserCtx.IffFile.Write(new SerializationContext(writer, deserCtx.IffFile, deserCtx.VersionInfo));
                    fs.Write(writer.Data, 0, deserCtx.IffFile.Form.Size + 8);
                    newPath = tmpFile;
                }

                return modified;
            }
        }

    TreatAsRegular:
        {
            var modifiable = false;
            foreach (var fileModifier in fileModifiers)
                modifiable |= fileModifier.CanModify(path, context);

            if (modifiable) {
                var modified = false;

                using var fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                var data = new byte[fs.Length];
                var read = fs.Read(data, 0, data.Length);
                if (read != data.Length)
                    throw new IOException($"Failed to read {data.Length} bytes from {path}, only read {read} bytes.");

                foreach (var fileModifier in fileModifiers)
                    modified |= fileModifier.ModifyFile(path, context, ref data);

                if (modified) {
                    using var fs2 = File.Open(tmpFile, FileMode.Create, FileAccess.Write, FileShare.None);
                    fs2.Write(data, 0, data.Length);
                    newPath = tmpFile;
                }

                return modified;
            }
        }

        return false;
    }

    private static string GetTmpFileAndCleanUpLeftoverTmpFiles(string path) {
        const string tmp = "tmp";

        var dirName = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();
        var fileName = Path.GetFileNameWithoutExtension(path);
        var fileExtension = Path.GetExtension(path);

        foreach (var file in Directory.EnumerateFiles(dirName, $"{fileName}.{tmp}*{fileExtension}")) {
            try {
                File.Delete(file);
            }
            catch (Exception e) {
                Console.WriteLine($"Failed to delete {file}: {e}");
            }
        }
        
        var tmpCount = 0;
        while (File.Exists($"{fileName}.{tmp}{tmpCount}{fileExtension}"))
            tmpCount++;
        
        return Path.Combine(dirName, $"{fileName}.{tmp}{tmpCount}{fileExtension}");
    }

    private static bool IsIff(string path, FileContext context) {
        return context == FileContext.Bundle && known_iff_extensions.Contains(Path.GetExtension(path));
    }
}
