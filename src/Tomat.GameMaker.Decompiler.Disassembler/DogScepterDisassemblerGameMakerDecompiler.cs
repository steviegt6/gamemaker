using System.Globalization;
using System.Text;
using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks.CODE;
using Tomat.GameMaker.IFF.Chunks.STRG;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;

namespace Tomat.GameMaker.Decompiler.Disassembler;

/// <summary>
///     Disassembles binary GameMaker bytecode to DogScepter assembly.
/// </summary>
public sealed class DogScepterDisassemblerGameMakerDecompiler : IGameMakerDecompiler {
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

    public static readonly Dictionary<ushort, string> BREAK_ID_TO_NAME = new() {
        { (ushort)GameMakerInstructionBreakType.Chkindex,    "chkindex" },
        { (ushort)GameMakerInstructionBreakType.Pushaf,      "pushaf" },
        { (ushort)GameMakerInstructionBreakType.Popaf,       "popaf" },
        { (ushort)GameMakerInstructionBreakType.Pushac,      "pushac" },
        { (ushort)GameMakerInstructionBreakType.Setowner,    "setowner" },
        { (ushort)GameMakerInstructionBreakType.Isstaticok,  "isstaticok" },
        { (ushort)GameMakerInstructionBreakType.Setstatic,   "setstatic" },
        { (ushort)GameMakerInstructionBreakType.Savearef,    "savearef" },
        { (ushort)GameMakerInstructionBreakType.Restorearef, "restorearef" },
        { (ushort)GameMakerInstructionBreakType.Isnullish,   "isnullish" },
    };

    public DecompilerResult DecompileFunction(DecompilerContext context, GameMakerCode code) {
        var result = new DecompilerResult("#");
        var bytecode = code.BytecodeEntry;
        if (bytecode is null)
            return result.WithError("Bytecode entry was null.");

        var strings = context.DeserializationContext.IffFile.GetChunk<GameMakerStrgChunk>().Strings;

        var sb = new StringBuilder();

        sb.AppendLine($"# {code.Name.ExpectObject().Value}");
        sb.AppendLine($"# 0x{code.BytecodeOffset:X8}");

        var blocks = FindBlockAddresses(bytecode, result);

        for (var i = 0; i < bytecode.Instructions.Count; i++) {
            var instruction = bytecode.Instructions[i];
            var addressIndex = blocks.IndexOf(instruction.Address);

            if (addressIndex != -1) {
                sb.AppendLine();
                sb.AppendLine($":[{addressIndex}]");
            }
            else {
                result.WithWarning($"0x{i:X8}: No block address found for instruction address 0x{instruction.Address:X8}.");
            }

            // If not a break opcode, print the opcode name (without data
            // types).
            if (instruction.Opcode != GameMakerCodeInstructionOpcode.Break)
                sb.Append(instruction.Opcode.ToString().ToLower());

            switch (GameMakerCodeInstruction.GetInstructionType(instruction.Opcode)) {
                case GameMakerCodeInstructionType.SingleType:
                    sb.Append($".{DATA_TYPE_TO_CHAR[instruction.DataType1]}");

                    if (instruction.Opcode == GameMakerCodeInstructionOpcode.CallV) {
                        sb.Append($" {instruction.Extra}");
                    }
                    else if (instruction.Opcode == GameMakerCodeInstructionOpcode.Dup) {
                        sb.Append($" {instruction.Extra}");

                        if (instruction.ComparisonType != 0)
                            sb.Append($" {(byte)instruction.ComparisonType & 0x7F}");
                    }

                    break;

                case GameMakerCodeInstructionType.DoubleType:
                    sb.Append($".{DATA_TYPE_TO_CHAR[instruction.DataType1]}.{DATA_TYPE_TO_CHAR[instruction.DataType2]}");
                    break;

                case GameMakerCodeInstructionType.Comparison:
                    sb.Append($".{DATA_TYPE_TO_CHAR[instruction.DataType1]}.{DATA_TYPE_TO_CHAR[instruction.DataType2]} {instruction.ComparisonType.ToString().ToUpper()}");
                    break;

                case GameMakerCodeInstructionType.Branch:
                    if (instruction.Address + (instruction.JumpOffset * 4) == code.Length) {
                        sb.Append(" [end]");
                    }
                    else if (instruction.PopenvExitMagic) {
                        // Magic instruction when returning early inside a with statement.
                        sb.Append( "[magic]");
                    }
                    else {
                        var index = blocks.IndexOf(instruction.Address + (instruction.JumpOffset * 4));

                        if (index == -1) {
                            result.WithError($"0x{i:X8}: No block address found for instruction address 0x{instruction.Address + (instruction.JumpOffset * 4):X8}.");
                            sb.Append(" [<error: unknown block index>]");
                        }
                        else {
                            sb.Append($" [{index}]");
                        }
                    }

                    break;

                case GameMakerCodeInstructionType.Pop:
                    sb.Append($".{DATA_TYPE_TO_CHAR[instruction.DataType1]}.{DATA_TYPE_TO_CHAR[instruction.DataType2]} ");

                    if (instruction.DataType1 == GameMakerInstructionDataType.Int16) {
                        // Special swap instruction.
                        sb.Append(((short)instruction.InstanceType).ToString().ToLower());
                    }
                    else {
                        if (instruction.DataType1 == GameMakerInstructionDataType.Variable && instruction.InstanceType != GameMakerCodeInstanceType.Undefined)
                            sb.Append($"{instruction.InstanceType.ToString().ToLower()}.");

                        sb.Append(StringifyVariableReference(instruction.Variable, i, result));
                    }

                    break;

                case GameMakerCodeInstructionType.Push:
                    sb.Append($".{DATA_TYPE_TO_CHAR[instruction.DataType1]} ");

                    if (instruction.DataType1 == GameMakerInstructionDataType.Variable) {
                        if (instruction.InstanceType != GameMakerCodeInstanceType.Undefined)
                            sb.Append($"{instruction.InstanceType.ToString().ToLower()}.");

                        sb.Append(StringifyVariableReference(instruction.Variable, i, result));
                    }
                    else if (instruction.DataType1 == GameMakerInstructionDataType.String) {
                        if (instruction.Value is null) {
                            result.WithError($"0x{i:X8}: Push instruction had no value.");
                            sb.Append("<error: null string>");
                        }
                        else {
                            var str = strings[(int)instruction.Value];

                            if (str.IsNull || str.ExpectObject().Value is null) {
                                result.WithError($"0x{i:X8}: Push instruction had null string.");
                                sb.Append("<error: null string>");
                            }
                            else {
                                sb.Append($"\"{SanitizeString(str.ExpectObject().Value!)}\"");
                            }
                        }
                    }
                    else if (instruction.Function is not null) {
                        if (instruction.Function?.Target?.Name is null || instruction.Function.Target.Name.IsNull) {
                            result.WithError($"0x{i:X8}: Push instruction had no function name.");
                            sb.Append("<error: null function or null function name>");
                        }
                        else {
                            sb.Append(instruction.Function.Target.Name.ExpectObject().Value);
                        }
                    }
                    else {
                        if (instruction.Value is null) {
                            result.WithError($"0x{i:X8}: Push instruction had no value.");
                            sb.Append("<error: null string>");
                        }
                        else
                            sb.Append((instruction.Value as IFormattable)?.ToString(null, CultureInfo.InvariantCulture) ?? instruction.Value.ToString());
                    }

                    break;

                case GameMakerCodeInstructionType.Call:
                    string? name = null;

                    if (instruction.Function?.Target?.Name is null || instruction.Function.Target.Name.IsNull) {
                        result.WithError($"0x{i:X8}: Call instruction had no name.");
                        sb.Append($"<error: unknown break type>.{DATA_TYPE_TO_CHAR[instruction.DataType1]}");
                    }
                    else {
                        name = instruction.Function.Target.Name.ExpectObject().Value;
                    }

                    string? value = null;

                    if (instruction.Value is null) {
                        result.WithError($"0x{i:X8}: Call instruction had no value.");
                        sb.Append($"<error: unknown break type>.{DATA_TYPE_TO_CHAR[instruction.DataType1]}");
                    }
                    else {
                        value = ((short)instruction.Value).ToString();
                    }

                    sb.Append($".{DATA_TYPE_TO_CHAR[instruction.DataType1]} {name ?? "<error: null function name>"} {value ?? "<error: unknown value>"}");
                    break;

                case GameMakerCodeInstructionType.Break:
                    if (instruction.Value is null) {
                        result.WithError($"0x{i:X8}: Break instruction had no value.");
                        sb.Append($"<null: unknown break type>.{DATA_TYPE_TO_CHAR[instruction.DataType1]}");
                    }
                    else {
                        sb.Append($"{BREAK_ID_TO_NAME[(ushort)instruction.Value]}.{DATA_TYPE_TO_CHAR[instruction.DataType1]}");
                    }

                    break;
            }

            // Finalize the line.
            sb.AppendLine();
        }

        sb.AppendLine();
        sb.AppendLine(":[end]");

        return result.WithCode(sb.ToString());
    }

    private static List<int> FindBlockAddresses(GameMakerCodeBytecode bytecode, DecompilerResult result) {
        var addresses = new HashSet<int>();

        if (bytecode.Instructions.Count == 0)
            return addresses.ToList();

        // TODO: Use offset/address instead?
        addresses.Add(0);

        for (var i = 0; i < bytecode.Instructions.Count; i++) {
            var instruction = bytecode.Instructions[i];

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

                case GameMakerCodeInstructionOpcode.Call:
                    if (i < 4)
                        break;

                    var name = instruction.Function?.Target?.Name;
                    if (name is null || name.Value.IsNull)
                        break;

                    if (name.Value.ExpectObject().Value != "@@try_hook@@")
                        break;

                    if (bytecode.Instructions[i - 4].Value is not int finallyBlock) {
                        result.WithWarning($"0x{i:X8}: Unresolved finally block, expected instruction at 0x{i - 4:X8} to have an int32 value.");
                        break;
                    }

                    addresses.Add(finallyBlock);

                    if (bytecode.Instructions[i - 2].Value is not int catchBlock) {
                        result.WithWarning($"0x{i:X8}: Unresolved catch block, expected instruction at 0x{i - 2:X8} to have an int32 value.");
                        break;
                    }

                    if (catchBlock != -1)
                        addresses.Add(catchBlock);

                    // According to DogScepter, there is not usually a block
                    // here (before/after the call), but for our purposes, this
                    // is easier to split into its own section to isolate it
                    // now.
                    addresses.Add(instruction.Address - 24);
                    addresses.Add(instruction.Address + 12);
                    break;
            }
        }

        var sorted = addresses.ToList();
        sorted.Sort();
        return sorted;
    }

    private static string SanitizeString(string str) {
        return str.Replace("\\", "\\\\");
    }

    private static string StringifyVariableReference(GameMakerCodeInstructionReference<GameMakerVariable>? variable, int index, DecompilerResult result) {
        if (variable is null) {
            result.WithError($"0x{index:X8}: Variable reference was null.");
            return "<error: null variable reference>";
        }

        string variableTargetName;

        if (variable.Target?.Name is null || variable.Target.Name.IsNull || variable.Target.Name.ExpectObject().Value is null) {
            result.WithError($"0x{index:X8}: Variable reference had no name.");
            variableTargetName = "<error: null variable name>";
        }
        else {
            variableTargetName = variable.Target.Name.ExpectObject().Value!;
        }

        if (variable.VariableType != GameMakerCodeInstructionVariableType.Normal) {
            string variableTypeName;

            if (variable.Target is null) {
                result.WithError($"0x{index:X8}: Variable reference had no target.");
                variableTypeName = "<error: null variable target>";
            }
            else {
                variableTypeName = variable.Target.VariableType.ToString().ToLower();
            }

            return $"[{variable.VariableType.ToString().ToLower()}]{variableTypeName}.{variableTargetName}";
        }

        return variableTargetName;
    }
}
