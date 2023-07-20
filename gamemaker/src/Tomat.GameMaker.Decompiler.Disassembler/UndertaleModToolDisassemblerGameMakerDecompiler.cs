using System.Globalization;
using System.Text;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks.CODE;
using Tomat.GameMaker.IFF.Chunks.FUNC;
using Tomat.GameMaker.IFF.Chunks.VARI;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;
using Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;
using Tomat.GameMaker.IFF.DataTypes.Models.Local;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;

namespace Tomat.GameMaker.Decompiler.Disassembler;

/// <summary>
///     Disassembles binary GameMaker bytecode to UndertaleModTool assembly.
/// </summary>
public sealed class UndertaleModToolDisassemblerGameMakerDecompiler : IGameMakerDecompiler {
    public static readonly Dictionary<short, string> BREAK_ID_TO_NAME = new() {
        { -1,  "chkindex" },
        { -2,  "pushaf" },
        { -3,  "popaf" },
        { -4,  "pushac" },
        { -5,  "setowner" },
        { -6,  "isstaticok" },
        { -7,  "setstatic" },
        { -8,  "savearef" },
        { -9,  "restorearef" },
        { -10, "chknullish" },
    };

    public static readonly Dictionary<string, short> NAME_TO_BREAK_ID = new() {
        { "chkindex",    -1 },
        { "pushaf",      -2 },
        { "popaf",       -3 },
        { "pushac",      -4 },
        { "setowner",    -5 },
        { "isstaticok",  -6 },
        { "setstatic",   -7 },
        { "savearef",    -8 },
        { "restorearef", -9 },
        { "chknullish",  -10 },
    };

    public static readonly Dictionary<GameMakerInstructionDataType, char> DATA_TYPE_TO_CHAR = new() {
        { GameMakerInstructionDataType.Double,   'd' },
        { GameMakerInstructionDataType.Float,    'f' },
        { GameMakerInstructionDataType.Int32,    'i' },
        { GameMakerInstructionDataType.Int64,    'l' },
        { GameMakerInstructionDataType.Boolean,  'b' },
        { GameMakerInstructionDataType.Variable, 'v' },
        { GameMakerInstructionDataType.String,   's' },
        { GameMakerInstructionDataType.Int16,    'e' },
    };

    public static readonly Dictionary<char, GameMakerInstructionDataType> CHAR_TO_DATA_TYPE = new() {
        { 'd', GameMakerInstructionDataType.Double },
        { 'f', GameMakerInstructionDataType.Float },
        { 'i', GameMakerInstructionDataType.Int32 },
        { 'l', GameMakerInstructionDataType.Int64 },
        { 'b', GameMakerInstructionDataType.Boolean },
        { 'v', GameMakerInstructionDataType.Variable },
        { 's', GameMakerInstructionDataType.String },
        { 'e', GameMakerInstructionDataType.Int16 },
    };

    public DecompilerResult DecompileFunction(DecompilerContext context, GameMakerCode code) {
        var result = new DecompilerResult(";");
        var bytecode = code.BytecodeEntry;
        if (bytecode is null)
            return result.WithError("Bytecode entry was null.");

        var locals = context.DeserializationContext.IffFile.GetChunk<GameMakerFuncChunk>().Locals;
        var variables = context.DeserializationContext.IffFile.GetChunk<GameMakerVariChunk>().Variables;
        var codeLocals = locals.FirstOrDefault(x => x.Name.ExpectObject().Value == code.Name.ExpectObject().Value);

        var sb = new StringBuilder();

        // UMT checks for 'WeirdLocalFlag' here as well, see the associated
        // comment in GenerateLocalVarDefinitions for why we don't.
        if (codeLocals is null)
            result.WithWarning("Could not find locals entry for code entry.");
        else
            sb.Append(GenerateLocalVarDefinitions(code, variables, codeLocals, result));

        var fragments = new Dictionary<int, string>();

        foreach (var child in code.Children) {
            // TODO: Warn when childName is null?
            string? childName = null;
            if (!child.Name.IsNull && child.Name.ExpectObject().Value is not null)
                childName = child.Name.ExpectObject().Value;

            childName ??= "<null>";

            // UMT divides offset by 4, we don't need to.
            fragments.Add(child.BytecodeOffset, $"{childName} (locals={child.LocalsCount}, argc={child.ArgumentsCount})");
        }

        var blocks = FindBlockAddress(bytecode, result);

        foreach (var instruction in bytecode.Instructions) {
            var doNewLine = true;

            if (fragments.TryGetValue(instruction.Address, out var entry)) {
                sb.AppendLine();
                sb.AppendLine($"> {entry}");
                doNewLine = false;
            }

            var index = blocks.IndexOf(instruction.Address);

            if (index != -1) {
                if (doNewLine)
                    sb.AppendLine();
                sb.AppendLine($":[{index}]");
            }

            sb.AppendLine(StringifyInstruction(instruction, code, blocks, result));
        }

        sb.AppendLine();
        sb.AppendLine(":[end]");

        return result.WithCode(sb.ToString());
    }

    private static List<int> FindBlockAddress(GameMakerCodeBytecode bytecode, DecompilerResult result) {
        var addresses = new HashSet<int>();

        if (bytecode.Instructions.Count != 0)
            addresses.Add(0);

        foreach (var instruction in bytecode.Instructions) {
            switch (instruction.Opcode) {
                case GameMakerCodeInstructionOpcode.B:
                case GameMakerCodeInstructionOpcode.Bf:
                case GameMakerCodeInstructionOpcode.Bt:
                case GameMakerCodeInstructionOpcode.PushEnv:
                    addresses.Add(instruction.Address + 4);
                    addresses.Add(instruction.Address + (instruction.JumpOffset * 4));
                    break;

                case GameMakerCodeInstructionOpcode.PopEnv:
                    if (!instruction.PopenvExitMagic)
                        addresses.Add(instruction.Address + (instruction.JumpOffset * 4));
                    break;

                case GameMakerCodeInstructionOpcode.Exit:
                case GameMakerCodeInstructionOpcode.Ret:
                    addresses.Add(instruction.Address + 4);
                    break;
            }
        }

        var sorted = addresses.ToList();
        sorted.Sort();
        return sorted;
    }

    private static string? GenerateLocalVarDefinitions(GameMakerCode code, List<GameMakerVariable>? variables, GameMakerLocalsEntry? locals, DecompilerResult result) {
        // In UMT, they check whether 'WeirdLocalFlags' is true and returns an
        // empty string if it is. As far as I can tell, this is legacy and no
        // longer gets set, so we can ignore it here (no equivalent exists in
        // our API).

        if (variables is null) {
            result.WithError("Missing master (IFF) variable list -- this is a bug.");
            return null;
        }

        if (locals is null) {
            result.WithWarning("Missing code locals -- possibly due to unsupported bytecode version or brand new code entry.");
            return null;
        }

        if (locals.Entries is null) {
            result.WithError("Missing code locals entries.");
            return null;
        }

        // This should never actually occur since we check this before this
        // method is called, but better safe than sorry.
        if (code.BytecodeEntry is null) {
            result.WithError("Missing bytecode entry.");
            return null;
        }

        var sb = new StringBuilder();
        var referenced = FindReferencedLocalVariables(code.BytecodeEntry);

        if (locals.Name.ExpectObject().Value != code.Name.ExpectObject().Value) {
            result.WithError("Code name and locals name mismatch.");
            return null;
        }

        foreach (var argument in locals.Entries) {
            sb.Append(".localvar " + argument.Index + " " + argument.Name.ExpectObject().Value);
            var refVar = referenced.FirstOrDefault(x => x.Name.ExpectObject().Value == argument.Name.ExpectObject().Value && x.VariableId == argument.Index);

            // Maybe log when no matches?
            if (refVar is not null)
                sb.Append(" " + variables.IndexOf(refVar));

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static List<GameMakerVariable> FindReferencedVariables(GameMakerCodeBytecode bytecode) {
        var variables = new List<GameMakerVariable>();

        foreach (var instruction in bytecode.Instructions) {
            var variable = instruction.Variable?.Target;
            if (variable is not null && !variables.Contains(variable))
                variables.Add(variable);
        }

        return variables;
    }

    private static List<GameMakerVariable> FindReferencedLocalVariables(GameMakerCodeBytecode bytecode) {
        return FindReferencedVariables(bytecode).Where(x => x.VariableType == GameMakerCodeInstanceType.Local).ToList();
    }

    private static string StringifyInstruction(GameMakerCodeInstruction instruction, GameMakerCode? code, List<int>? blocks, DecompilerResult result) {
        var sb = new StringBuilder();

        var kind = instruction.Opcode.ToString();
        var type = GameMakerCodeInstruction.GetInstructionType(instruction.Opcode);
        var unknownBreak = false;

        if (type == GameMakerCodeInstructionType.Break) {
            // TODO: Our special case for safety, warn?
            if (instruction.Value is not short value) {
                kind = kind.ToLower(CultureInfo.InvariantCulture);
                unknownBreak = true;
            }
            else if (BREAK_ID_TO_NAME.TryGetValue(value, out var breakKind)) {
                kind = breakKind;
            }
            else {
                kind = kind.ToLower(CultureInfo.InvariantCulture);
                unknownBreak = true;
            }
        }
        else {
            kind = kind.ToLower(CultureInfo.InvariantCulture);
        }

        sb.Append(kind);

        switch (GameMakerCodeInstruction.GetInstructionType(instruction.Opcode)) {
            case GameMakerCodeInstructionType.SingleType:
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType1]);

                if (instruction.Opcode is GameMakerCodeInstructionOpcode.Dup or GameMakerCodeInstructionOpcode.CallV) {
                    sb.Append(' ');
                    sb.Append(instruction.Extra);

                    if (instruction.Opcode == GameMakerCodeInstructionOpcode.Dup && instruction.ComparisonType != 0) {
                        // Special dup instruction with extra parameters.
                        sb.Append(' ');
                        sb.Append((byte)instruction.ComparisonType & 0x7F);
                        sb.Append(" ;;; This is a weird GMS2.3+ swap instruction.");
                    }
                }

                break;

            case GameMakerCodeInstructionType.DoubleType:
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType1]);
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType2]);
                break;

            case GameMakerCodeInstructionType.Comparison:
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType1]);
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType2]);
                sb.Append(' ');
                sb.Append(instruction.ComparisonType.ToString().ToUpper());
                break;

            case GameMakerCodeInstructionType.Branch:
                sb.Append(' ');
                string tgt;
                if (code is not null && instruction.Address + instruction.JumpOffset == code.Length / 4)
                    tgt = "[end]";
                else if (instruction.PopenvExitMagic)
                    tgt = "<drop>";
                else if (blocks is not null)
                    tgt = "[" + blocks.IndexOf(instruction.Address + (instruction.JumpOffset * 4)) + ']';
                else
                    tgt = (instruction.Address + instruction.JumpOffset).ToString("D5");
                sb.Append(tgt);
                break;

            case GameMakerCodeInstructionType.Pop:
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType1]);
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType2]);
                sb.Append(' ');

                if (instruction.DataType1 == GameMakerInstructionDataType.Int16) {
                    // Special scenario for the swap instruction.
                    // TODO: UMT needs to figure out proper syntax (and that
                    // means so do we), see UMT-129.
                    sb.Append((short)instruction.InstanceType);
                    sb.Append(" ;;; This is a weird GMS2.3+ swap instruction.");
                }
                else {
                    if (instruction.DataType1 == GameMakerInstructionDataType.Variable && instruction.InstanceType != GameMakerCodeInstanceType.Undefined) {
                        sb.Append(GameMakerCodeInstanceTypeToString(instruction.InstanceType));
                        sb.Append('.');
                    }

                    sb.Append(StringifyVariableRef(instruction.Variable));
                }

                break;

            case GameMakerCodeInstructionType.Push:
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType1]);
                sb.Append(' ');

                if (instruction.DataType1 == GameMakerInstructionDataType.Variable && instruction.InstanceType != GameMakerCodeInstanceType.Undefined) {
                    sb.Append(GameMakerCodeInstanceTypeToString(instruction.InstanceType));
                    sb.Append('.');
                }

                if (instruction.Value is not null)
                    sb.Append((instruction.Value as IFormattable)?.ToString(null, CultureInfo.InvariantCulture) ?? instruction.Value.ToString());
                else if (instruction.Variable is not null)
                    sb.Append(StringifyVariableRef(instruction.Variable));
                else if (instruction.Function is not null)
                    sb.Append(StringifyFunctionRef(instruction.Function));
                // TODO: error here
                break;

            case GameMakerCodeInstructionType.Call:
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType1]);
                sb.Append(' ');
                sb.Append(StringifyFunctionRef(instruction.Function));
                sb.Append("(argc=");
                var argc = (short?)instruction.Value;
                if (argc is null)
                    sb.Append('?');
                else
                    sb.Append(argc.Value);
                sb.Append(')');
                if (argc is null)
                    sb.Append(" ;;; Unknown argument count, this is a bug.");
                break;

            case GameMakerCodeInstructionType.Break:
                sb.Append("." + DATA_TYPE_TO_CHAR[instruction.DataType1]);

                if (unknownBreak) {
                    sb.Append(' ');
                    sb.Append(instruction.Value);
                    sb.Append(" ;;; Unknown break instruction.");
                }

                break;
        }

        return sb.ToString();
    }

    private static string StringifyFunctionRef(GameMakerCodeInstructionReference<GameMakerFunctionEntry>? function) {
        if (function is null)
            return "(null) ;;; Function object was null, this is a bug.";

        return function.Target?.Name.IsNull ?? true ? "(null)" : function.Target.Name.ExpectObject().Value ?? "(null)";
    }

    private static string StringifyVariableRef(GameMakerCodeInstructionReference<GameMakerVariable>? variable) {
        if (variable is null)
            return "(null) ;;; Variable object was null, this is a bug.";

        if (variable.VariableType == GameMakerCodeInstructionVariableType.Normal) {
            if (variable.Target?.Name is null || variable.Target.Name.IsNull)
                return "(null)";

            return variable.Target.Name.ExpectObject().Value ?? "(null)";
        }

        var type = variable.VariableType.ToString().ToLower(CultureInfo.InvariantCulture);
        var instanceType = variable.Target?.VariableType.ToString().ToLower(CultureInfo.InvariantCulture) ?? "null";
        var variableString = (variable.Target?.Name.IsNull ?? true) ? "<NULL_VAR_NAME>" : variable.Target.Name.ExpectObject().Value ?? "<NULL_VAR_NAME>";
        return $"[{type}]{instanceType}.{variableString}";
    }

    private static string GameMakerCodeInstanceTypeToString(GameMakerCodeInstanceType type) {
        return type switch {
            GameMakerCodeInstanceType.Argument => "arg",
            _ => type.ToString().ToLower(),
        };
    }
}
