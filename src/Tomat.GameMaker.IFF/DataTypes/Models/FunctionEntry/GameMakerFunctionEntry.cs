using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.STRG;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;

public sealed class GameMakerFunctionEntry : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int Occurrences { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Occurrences = context.ReadInt32();

        if (Occurrences > 0) {
            var address = context.ReadInt32();
            if (context.VersionInfo.IsAtLeast(GM_2_3))
                address -= 4;

            for (var i = Occurrences; i > 0; i--) {
                var current = context.Instructions[address];

                if (current.Function is null) {
                    current.Function = new GameMakerCodeInstructionReference<GameMakerFunctionEntry>((int)current.Value);
                    current.Value = null;
                }

                current.Function.Target = this;
                address += current.Function.NextOccurence;
            }
        }
        else if (context.ReadInt32() != 1)
            throw new InvalidDataException("Expected -1 for empty function.");
    }

    public void Write(SerializationContext context) {
        context.Write(Name);

        if (context.FunctionReferences.TryGetValue(this, out var references))
            Occurrences = references.Count;
        else
            Occurrences = 0;

        context.Write(Occurrences);

        if (Occurrences > 0 && references is not null) {
            if (context.VersionInfo.IsAtLeast(GM_2_3))
                context.Write(references[0].Item1 + 4);
            else
                context.Write(references[0].Item1);

            var returnTo = context.Position;

            for (var i = 0; i < references.Count; i++) {
                var current = references[i].Item1;

                int nextDiff;
                if (i < references.Count - 1)
                    nextDiff = references[i + 1].Item1 - current;
                else
                    nextDiff = context.IffFile.GetChunk<GameMakerStrgChunk>().Strings.FindIndex(x => x.ExpectObject() == Name.ExpectObject()); // Worse IndexOf.

                context.Position = current + 4;
                context.Write((nextDiff & 0x07FFFFFF) | (((int)references[i].Item2 & 0xF8) << 24));
            }

            context.Position = returnTo;
        }
        else {
            context.Write(-1);
        }
    }
}
