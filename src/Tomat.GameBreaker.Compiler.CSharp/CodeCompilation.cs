using System;
using Mono.Cecil;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.GEN8;

namespace Tomat.GameBreaker.Compiler.CSharp;

public class CodeCompilation {
    private readonly DeserializationContext context;

    public GameMakerIffFile Iff => context.IffFile;

    public CodeCompilation(DeserializationContext context) {
        this.context = context;
    }

    public AssemblyDefinition Compile() {
        var name = Iff.GetChunk<IGen8Chunk>().DisplayName.ExpectObject().Value;
        var asmNameDef = new AssemblyNameDefinition(name, new Version(1, 0, 0, 0));
        var asmDef = AssemblyDefinition.CreateAssembly(asmNameDef, name, ModuleKind.Dll);

        return asmDef;
    }
}
