using System;
using System.IO;
using Tomat.GameMaker.IFF.Chunks.STRG;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Variable;

public sealed class GameMakerVariable : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public CodeInstanceType VariableType { get; set; }

    public int VariableId { get; set; }

    public int Occurrences { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();

        if (context.VersionInfo.FormatId > 14) {
            VariableType = (CodeInstanceType)context.ReadInt32();

            // TODO: Handle max struct ID detection for compilation.

            VariableId = context.ReadInt32();
        }

        Occurrences = context.ReadInt32();

        if (Occurrences > 0) {
            var address = context.ReadInt32();

            // Parse reference chain.

            for (var i = Occurrences; i > 0; i--) {
                var current = context.Instructions[address];
                if (current.Variable is null)
                    throw new InvalidOperationException("Expected variable reference.");

                current.Variable.Target = this;
                address += current.Variable.NextOccurence;
            }
        }
        else if (context.ReadInt32() != -1) {
            throw new InvalidDataException("Expected -1 for empty variable.");
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Name);

        if (context.VersionInfo.FormatId > 14) {
            context.Write((int)VariableType);
            context.Write(VariableId);
        }

        if (context.VariableReferences.TryGetValue(this, out var references))
            Occurrences = references.Count;
        else
            Occurrences = 0;

        context.Write(Occurrences);
        if (Occurrences > 0 && references is not null) {
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
