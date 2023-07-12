using System;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;

namespace Tomat.GameMaker.IFF.Chunks.CODE;

public sealed class GameMakerCodeChunk : AbstractChunk {
    public const string NAME = "CODE";

    public GameMakerPointerList<GameMakerCode>? Code { get; set; }

    public GameMakerCodeChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        if (Size == 0)
            return; // In YYC, before bytecode 17, CODE is empty.

        Code = context.ReadPointerList<GameMakerCode>();
    }

    public override void Write(SerializationContext context) {
        if (Code is null)
            return;

        if (context.VersionInfo.FormatId <= 14) {
            context.Write(Code);
        }
        else {
            // Serialize bytecode before entries.
            context.Write(
                Code,
                beforeWriter: (ctx, i, _) => {
                    if (i != 0)
                        return;

                    foreach (var code in Code) {
                        var entry = code.ExpectObject().BytecodeEntry;

                        if (entry is null)
                            throw new InvalidOperationException("Expected bytecode entry.");

                        // Same as pointers, but not wrapped in a pointer helper
                        // struct.
                        ctx.Pointers[entry] = ctx.Position;
                        entry.Write(ctx);
                    }
                }
            );
        }
    }
}
