using ICSharpCode.Decompiler.Metadata;
using Mono.Cecil;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks.CODE;
using Tomat.GameMaker.IFF.Chunks.GEN8;
using Tomat.GameMaker.IFF.Chunks.OBJT;
using Tomat.GameMaker.IFF.Chunks.SCPT;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;

namespace Tomat.GameMaker.Decompiler.CSharp;

public sealed class CSharpGameMakerDecompiler : IGameMakerDecompiler {
    /// <summary>
    ///     A map of <see cref="DecompilerContext"/>s to <see cref="PEFile"/>.
    ///     This is used primarily to allow the
    ///     <see cref="CSharpGameMakerDecompiler"/> to handle binary file
    ///     creation as well as result caching to avoid expensive calls.
    /// </summary>
    public Dictionary<DecompilerContext, PEFile> PeFiles { get; set; } = new();

    public DecompilerResult DecompileFunction(DecompilerContext context, GameMakerCode code) {
        var peFile = GetOrCreatePeFile(context);
        throw new NotImplementedException();
    }

    public Dictionary<string, DecompilerResult> DecompileIffFile(DecompilerContext context) {
        // Debugging sanity checks.
        /*var codeChunk = context.DeserializationContext.IffFile.GetChunk<GameMakerCodeChunk>();

        var hasParent = new List<GameMakerCode>();
        var hasChildren = new List<GameMakerCode>();

        foreach (var code in codeChunk.Code!) {
            var codeObj = code.ExpectObject();
            if (codeObj.ParentEntry is not null)
                hasParent.Add(codeObj);

            if (codeObj.Children.Count != 0)
                hasChildren.Add(codeObj);
        }

        foreach (var code in hasParent) {
            if (code.Children.Count != 0)
                ;
        }

        foreach (var code in hasChildren) {
            if (code.ParentEntry is not null)
                ;
        }*/
        var peFile = GetOrCreatePeFile(context);
        throw new NotImplementedException();
    }

    private PEFile GetOrCreatePeFile(DecompilerContext context) {
        if (PeFiles.TryGetValue(context, out var peFile))
            return peFile;

        var gen8 = context.DeserializationContext.IffFile.GetChunk<GameMakerGen8Chunk>();
        var name = gen8.FileName.ExpectObject().Value!;
        using var moduleStream = ConvertIffFileToModuleStream(context, name);
        return PeFiles[context] = new PEFile(name + ".dll", moduleStream);
    }

    private MemoryStream ConvertIffFileToModuleStream(DecompilerContext context, string assemblyName) {
        var nameDef = new AssemblyNameDefinition(assemblyName, new Version(1, 0, 0, 0));
        var asmDef = AssemblyDefinition.CreateAssembly(nameDef, assemblyName, ModuleKind.Dll);
        var module = asmDef.MainModule;
        var ts = module.TypeSystem;

        // For GameMaker built-ins.
        const string gamemaker_namespace = "GameMaker";

        // For game-defined functions.
        var gameNamespace = assemblyName;
        var globalNamespace = gameNamespace + ".Global";
        var roomNamespace = gameNamespace + ".Room";
        var roomCcNamespace = gameNamespace + ".RoomCC";
        var objectNamespace = gameNamespace + ".Object";

        var iffFile = context.DeserializationContext.IffFile;
        /*var scptChunk = iffFile.GetChunk<GameMakerScptChunk>();
        var codeChunk = iffFile.GetChunk<GameMakerCodeChunk>();
        var globalScripts = scptChunk.Scripts.Where(x => !x.ExpectObject().Name.ExpectObject().Value!.StartsWith("gml_Script_")).ToList();
        var globalCode = globalScripts.ToDictionary(x => x.ExpectObject().Name.ExpectObject().Value!, x => codeChunk.Code![x.ExpectObject().CodeId].ExpectObject()).ToList();
        if (globalScripts.Count != globalCode.Count)
            throw new Exception("Global script count does not match global code count!");

        foreach (var (name, code) in globalCode) {
            var type = new TypeDefinition(globalNamespace, name, TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass, ts.Object);
            module.Types.Add(type);
        }

        var objtChunk = iffFile.GetChunk<GameMakerObjtChunk>();
        var objects = objtChunk.Objects.Select(x => x.ExpectObject()).ToList();

        foreach (var obj in objects) {
            var objectName = obj.Name.ExpectObject().Value!;
            var nameToTrim = $"gml_Object_{objectName}_";

            foreach (var evt in obj.Events.Select(x => x.ExpectObject()))
            foreach (var objEvt in evt.Select(x => x.ExpectObject()))
            foreach (var action in objEvt.Actions.Select(x => x.ExpectObject())) {
                var eventName = action.ActionName.ExpectObject().Value![nameToTrim.Length..];
            }
        }*/

        var codeChunk = iffFile.GetChunk<GameMakerCodeChunk>();
        var globalScripts = new List<GameMakerCode>();
        var roomScripts = new List<GameMakerCode>();
        var roomCcScripts = new List<GameMakerCode>();
        var objectScripts = new List<GameMakerCode>();
        var scripts = new List<GameMakerCode>();

        foreach (var code in codeChunk.Code!.Select(x => x.ExpectObject())) {
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

        TypeDefinition createType(string @namespace, string name, TypeSystem typeSystem) {
            return new TypeDefinition(
                @namespace,
                name,
                TypeAttributes.Class
              | TypeAttributes.Public
              | TypeAttributes.AutoLayout
              | TypeAttributes.AnsiClass
              | TypeAttributes.Abstract
              | TypeAttributes.Sealed
              | TypeAttributes.BeforeFieldInit,
                typeSystem.Object
            );
        }

        foreach (var globalScript in globalScripts) {
            var trimmedName = globalScript.Name.ExpectObject().Value!["gml_GlobalScript_".Length..];
            var type = createType(globalNamespace, trimmedName, ts);

            // Global scripts don't have any instructions not consumed by child
            // scripts, so we can just ignore them.

            foreach (var child in globalScript.Children) {
                var name = child.Name.ExpectObject().Value!["gml_Script_".Length..];
                var method = CreateMethod(name, child, ts);
                type.Methods.Add(method);
                scripts.Remove(child);
            }

            module.Types.Add(type);
        }

        foreach (var roomScript in roomScripts) {
            var trimmedName = roomScript.Name.ExpectObject().Value!["gml_Room_".Length..];
            var type = createType(roomNamespace, trimmedName, ts);

            var method = CreateMethod(trimmedName, roomScript, ts);
            type.Methods.Add(method);

            // Room scripts don't have children... I think?
            /*foreach (var child in roomScript.Children) {
                scripts.Remove(child);
            }*/

            module.Types.Add(type);
        }

        foreach (var roomCcScript in roomCcScripts) {
            var trimmedName = roomCcScript.Name.ExpectObject().Value!["gml_RoomCC_".Length..];
            var type = createType(roomCcNamespace, trimmedName, ts);

            var method = CreateMethod(trimmedName, roomCcScript, ts);
            type.Methods.Add(method);

            // RoomCC scripts don't have children... I think?
            /*foreach (var child in roomCcScript.Children) {
                scripts.Remove(child);
            }*/

            module.Types.Add(type);
        }

        foreach (var objectScript in objectScripts) {
            var trimmedName = objectScript.Name.ExpectObject().Value!["gml_Object_".Length..];
            var type = createType(objectNamespace, trimmedName, ts);

            var method = CreateMethod(trimmedName, objectScript, ts);
            type.Methods.Add(method);

            foreach (var child in objectScript.Children) {
                var childName = child.Name.ExpectObject().Value!["gml_Script_".Length..];
                var childMethod = CreateMethod(childName, child, ts);
                type.Methods.Add(childMethod);
                scripts.Remove(child);
            }

            module.Types.Add(type);
        }

        if (scripts.Count > 0)
            throw new Exception("Unknown/unhandled scripts found!");

        // write to file debug
        var ms = new MemoryStream();
        asmDef.Write(ms);
        File.WriteAllBytes(assemblyName + ".dll", ms.ToArray());

        ms.Seek(0, SeekOrigin.Begin);
        return ms;
    }

    private MethodDefinition CreateMethod(string name, GameMakerCode code, TypeSystem ts) {
        var method = new MethodDefinition(
            name,
            MethodAttributes.Public
          | MethodAttributes.HideBySig
          | MethodAttributes.Static,
            ts.Void
        );

        return method;
    }
}
