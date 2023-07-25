using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CppAst.CodeGen.Common;
using CppAst.CodeGen.CSharp;
using Zio.FileSystems;

namespace Tomat.Houli.BindGen;

internal static class Program {
    internal static void Main() {
        Console.WriteLine("Generated bindings...");

        GenerateBindings(new[] { Path.Combine("headers", "coreclr_delegates") }, "", "Tomat.Houli.Native.Import", "CoreclrDelegates");
        GenerateBindings(new[] { Path.Combine("headers", "hostfxr") }, "", "Tomat.Houli.Native.Import", "Hostfxr");
        GenerateBindings(new[] { Path.Combine("headers", "nethost") }, "", "Tomat.Houli.Native.Import", "Nethost");
    }

    private static void GenerateBindings(IEnumerable<string> headerDirectories, string libName, string outputNamespace, string outputClass) {
        var options = new CSharpConverterOptions {
            DefaultNamespace = outputNamespace,
            DefaultClassLib = outputClass,
            DefaultOutputFilePath = outputClass + ".cs",
            GenerateEnumItemAsFields = false,
            TypedefCodeGenKind = CppTypedefCodeGenKind.NoWrap,
            DefaultDllImportNameAndArguments = libName,
        };

        options.IncludeFolders.AddRange(headerDirectories);

        var headers = options.IncludeFolders.SelectMany(x => Directory.EnumerateFiles(x, "*.h"));
        var compilation = CSharpConverter.Convert(headers.ToList(), options);

        if (compilation.HasErrors) {
            Console.WriteLine("Completed with errors.");

            foreach (var error in compilation.Diagnostics.Messages)
                Console.WriteLine(error);

            return;
        }

        foreach (var error in compilation.Diagnostics.Messages)
            Console.WriteLine(error);

        using var fs = new PhysicalFileSystem();
        using var subFs = new SubFileSystem(fs, fs.ConvertPathFromInternal("."));

        var writer = new CodeWriter(new CodeWriterOptions(subFs));
        compilation.DumpTo(writer);
    }
}
