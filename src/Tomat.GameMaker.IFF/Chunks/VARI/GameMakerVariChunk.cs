using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks.FUNC;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;

namespace Tomat.GameMaker.IFF.Chunks.VARI;

public sealed class GameMakerVariChunk : AbstractChunk {
    public const string NAME = "VARI";

    public List<GameMakerVariable>? Variables { get; set; }

    public int VarCount1 { get; set; }

    public int VarCount2 { get; set; }

    public int MaxLocalVarCount { get; set; }

    public GameMakerVariChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        var startPos = context.Position;

        if (context.VersionInfo.FormatId > 14) {
            VarCount1 = context.ReadInt32();
            VarCount2 = context.ReadInt32();
            MaxLocalVarCount = context.ReadInt32();

            if (VarCount1 != VarCount2)
                context.VersionInfo.DifferentVarCounts = true;
        }

        var varLength = (context.VersionInfo.FormatId > 14) ? 20 : 12;

        Variables = new List<GameMakerVariable>();

        while (context.Position + varLength <= startPos + Size) {
            var variable = new GameMakerVariable();
            variable.Read(context);
            Variables.Add(variable);
        }
    }

    public override void Write(SerializationContext context) {
        if (context.VersionInfo.FormatId > 14) {
            // Count instance/global variables.
            if (context.VersionInfo.DifferentVarCounts) {
                VarCount1 = 0;
                VarCount2 = 0;

                foreach (var variable in Variables!) {
                    if (variable.VariableType == GameMakerCodeInstanceType.Global)
                        VarCount1++;
                    else if (variable is { VariableId: >= 0, VariableType: GameMakerCodeInstanceType.Self })
                        VarCount2++;
                }
            }
            else {
                VarCount1 = -1;

                foreach (var variable in Variables!) {
                    if (variable.VariableType is GameMakerCodeInstanceType.Global or GameMakerCodeInstanceType.Self)
                        VarCount1 = Math.Max(VarCount1, variable.VariableId);
                }

                VarCount1 += 1;
                VarCount2 = VarCount1;
            }

            context.Write(VarCount1);
            context.Write(VarCount2);

            // Set to highest amount of locals within all entries.
            MaxLocalVarCount = 0;
            foreach (var item in context.IffFile.GetChunk<GameMakerFuncChunk>().Locals)
                MaxLocalVarCount = Math.Max(MaxLocalVarCount, item.Entries!.Count);

            context.Write(MaxLocalVarCount);
        }

        foreach (var variable in Variables!)
            variable.Write(context);
    }
}
