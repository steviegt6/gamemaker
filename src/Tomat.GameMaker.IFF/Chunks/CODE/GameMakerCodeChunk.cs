using System;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;

namespace Tomat.GameMaker.IFF.Chunks.CODE;

internal sealed class GameMakerCodeChunk : AbstractChunk,
                                           ICodeChunk {
    public const string NAME = "CODE";

    public GameMakerCodeChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        if (Size == 0)
            return; // In YYC, before bytecode 17, CODE is empty.

        var code = context.ReadPointerList<GameMakerCode>();
        AddComponent<ICodeChunkCodeComponent>(new CodeChunkCodeComponent {
            Code = code,
        });
    }

    public override void Write(SerializationContext context) {
        if (!TryGetComponent<ICodeChunkCodeComponent>(out var codeComponent))
            return;

        var code = codeComponent.Code;

        if (context.VersionInfo.FormatId <= 14) {
            context.Write(code);
        }
        else {
            // Serialize bytecode before entries.
            context.Write(
                code,
                beforeWriter: (ctx, i, _) => {
                    if (i != 0)
                        return;

                    foreach (var codeEntry in code) {
                        var bytecode = codeEntry.ExpectObject().BytecodeEntry;

                        if (bytecode is null)
                            throw new InvalidOperationException("Expected bytecode entry.");

                        if (context.Pointers.ContainsKey(bytecode))
                            continue;

                        // Same as pointers, but not wrapped in a pointer helper
                        // struct.
                        ctx.Pointers[bytecode] = ctx.Position;
                        bytecode.Write(ctx);
                    }
                }
            );
        }
    }
}
