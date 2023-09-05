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
                    var tmpPath = path + ".tmp";
                    var tmpNum = 0;
                    while (File.Exists(tmpPath))
                        tmpPath = path + $".tmp{tmpNum++}";

                    using var fs = File.Open(tmpPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    var writer = new GameMakerIffWriter();
                    deserCtx.IffFile.Write(new SerializationContext(writer, deserCtx.IffFile, deserCtx.VersionInfo));
                    newPath = tmpPath;
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
                    var tmpPath = path + ".tmp";
                    var tmpNum = 0;
                    while (File.Exists(tmpPath))
                        tmpPath = path + $".tmp{tmpNum++}";

                    using var fs2 = File.Open(tmpPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    fs2.Write(data, 0, data.Length);
                    newPath = tmpPath;
                }

                return modified;
            }
        }

        return false;
    }

    private static bool IsIff(string path, FileContext context) {
        return context == FileContext.Bundle && known_iff_extensions.Contains(Path.GetExtension(path));
    }
}
