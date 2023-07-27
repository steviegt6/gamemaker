using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks.FUNC;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;

namespace Tomat.GameMaker.IFF.Chunks.VARI;

internal sealed class GameMakerVariChunk : AbstractChunk,
                                           IVariChunk {
    public const string NAME = "VARI";

    public List<GameMakerVariable> Variables { get; set; } = null!;

    public GameMakerVariChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        var startPos = context.Position;

        if (context.VersionInfo.FormatId > 14) {
            var varCount1 = context.ReadInt32();
            var varCount2 = context.ReadInt32();
            var maxLocalVarCount = context.ReadInt32();

            if (varCount1 != varCount2)
                context.VersionInfo.DifferentVarCounts = true;

            AddComponent<IVariChunkVariableCountComponent>(new VariChunkVariableCountComponent {
                VarCount1 = varCount1,
                VarCount2 = varCount2,
                MaxLocalVarCount = maxLocalVarCount,
            });
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
            if (!TryGetComponent<IVariChunkVariableCountComponent>(out var component))
                throw new InvalidOperationException("Missing component.");

            // Count instance/global variables.
            if (context.VersionInfo.DifferentVarCounts) {
                component.VarCount1 = 0;
                component.VarCount2 = 0;

                foreach (var variable in Variables!) {
                    if (variable.VariableType == GameMakerCodeInstanceType.Global)
                        component.VarCount1++;
                    else if (variable is { VariableId: >= 0, VariableType: GameMakerCodeInstanceType.Self })
                        component.VarCount2++;
                }
            }
            else {
                component.VarCount1 = -1;

                foreach (var variable in Variables!) {
                    if (variable.VariableType is GameMakerCodeInstanceType.Global or GameMakerCodeInstanceType.Self)
                        component.VarCount1 = Math.Max(component.VarCount1, variable.VariableId);
                }

                component.VarCount1 += 1;
                component.VarCount2 = component.VarCount1;
            }

            context.Write(component.VarCount1);
            context.Write(component.VarCount2);

            // Set to highest amount of locals within all entries.
            component.MaxLocalVarCount = 0;
            foreach (var item in context.IffFile.GetChunk<GameMakerFuncChunk>().Locals)
                component.MaxLocalVarCount = Math.Max(component.MaxLocalVarCount, item.Entries!.Count);

            context.Write(component.MaxLocalVarCount);
        }

        foreach (var variable in Variables!)
            variable.Write(context);
    }
}
