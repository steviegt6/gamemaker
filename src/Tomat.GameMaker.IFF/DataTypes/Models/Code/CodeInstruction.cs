using System;
using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Code;

public enum CodeInstructionKind : byte {
    Conv        = 0x07,
    Mul         = 0x08,
    Div         = 0x09,
    Rem         = 0x0A,
    Mod         = 0x0B,
    Add         = 0x0C,
    Sub         = 0x0D,
    And         = 0x0E,
    Or          = 0x0F,
    Xor         = 0x10,
    Neg         = 0x11,
    Not         = 0x12,
    Shl         = 0x13,
    Shr         = 0x14,
    Cmp         = 0x15,
    Pop         = 0x45,
    Dup         = 0x86,
    Ret         = 0x9C,
    Exit        = 0x9D,
    PopNull     = 0x9E,
    B           = 0xB6,
    Bt          = 0xB7,
    Bf          = 0xB8,
    PushEnv     = 0xBA,
    PopEnv      = 0xBB,
    Push        = 0xC0,
    PushLocal   = 0xC1,
    PushGlobal  = 0xC2,
    PushBuiltin = 0xC3,
    PushI       = 0x84,
    Call        = 0xD9,
    CallV       = 0x99,
    Break       = 0xFF,
}

public enum CodeInstructionComparisonType : byte {
    None = 0,
    Lt   = 1,
    Lte  = 2,
    Eq   = 3,
    Neq  = 4,
    Gte  = 5,
    Gt   = 6,
}

public enum CodeInstructionType : byte {
    Double,
    Float,
    Int32,
    Int64,
    Boolean,
    Variable,
    String,
    Instance,

    Delete,
    Undefined,
    UnsignedInt,
    Int16 = 0x0F,

    Unset = 0xFF,
}

public enum CodeInstanceType : short {
    // >= 0 are object indices, usually.
    Undefined    =  0,
    Self         = -1,
    Other        = -2,
    All          = -3,
    Noone        = -4,
    Global       = -5,
    NotSpecified = -6,
    Local        = -7,
    Ref          = -9,
    Argument     = -15,
    Static       = -16,
}

public enum CodeInstructionMetaKind {
    SingleType,
    DoubleType,
    Comparison,
    Branch,
    Push,
    Pop,
    Call,
    Break,
}

public enum CodeInstructionBreakType : ushort {
    IsNullish = 65526,
    RestoreARef = 65527,
    SaveARef = 65528,
    SetStatic = 65529,
    IsStaticOk = 65530,
    SetOwner = 65531,
    PushAc = 65532,
    PopAf = 65533,
    PushAf = 65534,
    ChkIndex = 65535,
}

internal sealed class GameMakerCodeInstruction : IGameMakerSerializable {
    public int Address { get; set; }

    public CodeInstructionKind Kind { get; set; }

    public CodeInstructionComparisonType ComparisonType { get; set; }

    public CodeInstructionType Type1 { get; set; }

    public CodeInstructionType Type2 { get; set; }

    public CodeInstanceType InstanceType { get; set; }

    public GameMakerCodeInstructionReference<GameMakerVariable>? Variable { get; set; }

    public GameMakerCodeInstructionReference<GameMakerFunctionEntry>? Function { get; set; }

    public object? Value { get; set; }

    public Int24 JumpOffset { get; set; }

    public bool PopenvExitMagic { get; set; }

    public byte Extra { get; set; }

    public int Length {
        get {
            if (Variable is not null || Function is not null)
                return 2;

            if (GetInstructionType(Kind) == CodeInstructionMetaKind.Push) {
                if (Type1 is CodeInstructionType.Double or CodeInstructionType.Int64)
                    return 3;

                if (Type1 != CodeInstructionType.Int16)
                    return 2;
            }

            return 1;
        }
    }

    public GameMakerCodeInstruction(int address) {
        Address = address;
    }

    public void Read(DeserializationContext context) {
        var start = context.Position;
        context.Instructions[start] = this;

        if (start % 4 != 0)
            throw new InvalidDataException("Instruction address is not aligned to 4 bytes.");

        // Read the opcode.
        context.Position += 3;
        var opcode = context.ReadByte();
        if (context.VersionInfo.FormatId <= 14)
            opcode = OldOpcodeToNew(opcode);
        Kind = (CodeInstructionKind)opcode;
        context.Position = start;

        switch (GetInstructionType(Kind)) {
            case CodeInstructionMetaKind.SingleType:
            case CodeInstructionMetaKind.DoubleType:
            case CodeInstructionMetaKind.Comparison: {
                Extra = context.ReadByte();

                if (Extra != 0 && Kind != CodeInstructionKind.Dup && Kind != CodeInstructionKind.CallV)
                    throw new InvalidDataException("Expected extra byte of 0 for opcode " + Kind + ", got " + Extra + ".");

                ComparisonType = (CodeInstructionComparisonType)context.ReadByte();

                var types = context.ReadByte();
                Type1 = (CodeInstructionType)(types & 0xF);
                Type2 = (CodeInstructionType)(types >> 4);
                if (Kind == CodeInstructionKind.Cmp && context.VersionInfo.FormatId <= 14)
                    ComparisonType = (CodeInstructionComparisonType)(context.ReadByte() - 0x10);
                else
                    context.Position += 1;

                if (Kind is CodeInstructionKind.And or CodeInstructionKind.Or) {
                    if (Type1 == CodeInstructionType.Boolean && Type2 == CodeInstructionType.Boolean)
                        context.VersionInfo.ShortCircuit = false;
                }

                break;
            }

            case CodeInstructionMetaKind.Branch: {
                if (context.VersionInfo.FormatId <= 14) {
                    JumpOffset = context.ReadInt24();
                    if (JumpOffset == -0x100000)
                        PopenvExitMagic = true;
                }
                else {
                    var v = context.ReadUInt24();
                    PopenvExitMagic = (v & 0x800000) != 0;

                    // The rest is int24 signed value, so make sure.
                    var r = v & 0x003FFFFF;
                    if ((v & 0x00C00000) != 0)
                        r |= 0xFFC00000;
                    JumpOffset = (int)r;
                }

                context.Position += 1;
                break;
            }

            case CodeInstructionMetaKind.Pop: {
                InstanceType = (CodeInstanceType)context.ReadInt16();

                var types = context.ReadByte();
                Type1 = (CodeInstructionType)(types & 0xF);
                Type2 = (CodeInstructionType)(types >> 4);

                context.Position += 1;

                // Ignore swap instructions.
                if (Type1 != CodeInstructionType.Int16) {
                    Variable = new GameMakerCodeInstructionReference<GameMakerVariable>();
                    Variable.Read(context);
                }

                break;
            }

            case CodeInstructionMetaKind.Push: {
                var val = context.ReadInt16();

                Type1 = (CodeInstructionType)context.ReadByte();

                if (context.VersionInfo.FormatId <= 14) {
                    if (Type1 == CodeInstructionType.Variable) {
                        Kind = val switch {
                            -5 => CodeInstructionKind.PushGlobal,
                            -6 => CodeInstructionKind.PushBuiltin,
                            -7 => CodeInstructionKind.PushLocal,
                            _ => Kind,
                        };
                    }
                    else if (Type1 == CodeInstructionType.Int16) {
                        Kind = CodeInstructionKind.PushI;
                    }
                }

                context.Position += 1;

                switch (Type1) {
                    case CodeInstructionType.Double:
                        Value = context.ReadDouble();
                        break;

                    case CodeInstructionType.Float:
                        Value = context.ReadSingle();
                        break;

                    case CodeInstructionType.Int32:
                        Value = context.ReadInt32();
                        break;

                    case CodeInstructionType.Int64:
                        Value = context.ReadInt64();
                        break;

                    case CodeInstructionType.Boolean:
                        Value = context.ReadBoolean(wide: true);
                        break;

                    case CodeInstructionType.Variable:
                        InstanceType = (CodeInstanceType)val;
                        Variable = new GameMakerCodeInstructionReference<GameMakerVariable>();
                        Variable.Read(context);
                        break;

                    case CodeInstructionType.String:
                        Value = context.ReadInt32(); // String ID.
                        break;

                    case CodeInstructionType.Int16:
                        Value = val;
                        break;
                }

                break;
            }

            case CodeInstructionMetaKind.Call: {
                Value = context.ReadInt16();
                Type1 = (CodeInstructionType)context.ReadByte();

                context.Position += 1;

                Function = new GameMakerCodeInstructionReference<GameMakerFunctionEntry>();
                Function.Read(context);
                break;
            }

            case CodeInstructionMetaKind.Break: {
                Value = context.ReadUInt16();
                Type1 = (CodeInstructionType)context.ReadByte();

                context.Position += 1;
                break;
            }

            default:
                throw new InvalidDataException($"Unknown instruction type {GetInstructionType(Kind)} from opcode{Kind}.");
        }
    }

    public void Write(SerializationContext context) {
        if (Variable is not null) {
            if (Variable.Target is not null) {
                if (context.VariableReferences.TryGetValue(Variable.Target, out var l)) {
                    l.Add((context.Position, Variable.VariableType));
                }
                else {
                    context.VariableReferences.Add(
                        Variable.Target,
                        new List<(int, GameMakerCodeInstructionVariableType)> {
                            (context.Position, Variable.VariableType),
                        }
                    );
                }
            }
            else {
                throw new InvalidOperationException($"Missing variable target at {context.Position}.");
            }
        }
        else if (Function is not null) {
            if (Function.Target is not null) {
                if (context.FunctionReferences.TryGetValue(Function.Target, out var l)) {
                    l.Add((context.Position, Function.VariableType));
                }
                else {
                    context.FunctionReferences.Add(
                        Function.Target,
                        new List<(int, GameMakerCodeInstructionVariableType)> {
                            (context.Position, Function.VariableType),
                        }
                    );
                }
            }
            else {
                throw new InvalidOperationException($"Missing function target at {context.Position}.");
            }
        }

        switch (GetInstructionType(Kind)) {
            case CodeInstructionMetaKind.SingleType:
            case CodeInstructionMetaKind.DoubleType:
            case CodeInstructionMetaKind.Comparison: {
                context.Write(Extra);
                if (context.VersionInfo.FormatId <= 14 && Kind == CodeInstructionKind.Cmp)
                    context.Write((byte)0);
                else
                    context.Write((byte)ComparisonType);
                context.Write((byte)((byte)Type2 << 4 | (byte)Type1));
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Kind, (byte)ComparisonType));
                else
                    context.Write((byte)Kind);
                break;
            }

            case CodeInstructionMetaKind.Branch: {
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(JumpOffset);
                else if (PopenvExitMagic)
                    context.Write((Int24)0xF00000);
                else
                    context.Write((Int24)(int)((uint)(int)JumpOffset & ~0xFF800000));

                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Kind, 0));
                else
                    context.Write((byte)Kind);
                break;
            }

            case CodeInstructionMetaKind.Pop: {
                context.Write((short)InstanceType);
                context.Write((byte)((byte)Type2 << 4 | (byte)Type1));
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Kind, 0));
                else
                    context.Write((byte)Kind);

                if (Type1 != CodeInstructionType.Int16) {
                    if (Variable is null)
                        throw new InvalidOperationException($"Missing variable at {context.Position}.");

                    Variable.Write(context);
                }

                break;
            }

            case CodeInstructionMetaKind.Push: {
                if (Type1 == CodeInstructionType.Int16)
                    context.Write(Convert.ToInt16(Value));
                else if (Type1 == CodeInstructionType.Variable)
                    context.Write((short)InstanceType);
                else
                    context.Write((short)0);

                context.Write((byte)Type1);

                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Kind, 0));
                else
                    context.Write((byte)Kind);

                switch (Type1) {
                    case CodeInstructionType.Double: {
                        context.Write(Convert.ToDouble(Value));
                        break;
                    }

                    case CodeInstructionType.Float: {
                        context.Write(Convert.ToSingle(Value));
                        break;
                    }

                    case CodeInstructionType.Int32: {
                        context.Write(Convert.ToInt32(Value));
                        break;
                    }

                    case CodeInstructionType.Int64: {
                        context.Write(Convert.ToInt64(Value));
                        break;
                    }

                    case CodeInstructionType.Boolean: {
                        context.Write(Convert.ToBoolean(Value), wide: true);
                        break;
                    }

                    case CodeInstructionType.Variable: {
                        if (Variable is null)
                            throw new InvalidOperationException($"Missing variable at {context.Position}.");

                        Variable.Write(context);
                        break;
                    }

                    case CodeInstructionType.String: {
                        context.Write(Convert.ToInt32(Value)); // String ID.
                        break;
                    }

                    // case GameMakerInstructionDataType.Int16:
                    //     break;
                }

                break;
            }

            case CodeInstructionMetaKind.Call: {
                context.Write(Convert.ToInt16(Value));
                context.Write((byte)Type1);
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Kind, 0));
                else
                    context.Write((byte)Kind);

                if (Function is null)
                    throw new InvalidOperationException($"Missing function at {context.Position}.");

                Function.Write(context);
                break;
            }

            case CodeInstructionMetaKind.Break: {
                context.Write(Convert.ToUInt16(Value));
                context.Write((byte)Type1);
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Kind, 0));
                else
                    context.Write((byte)Kind);
                break;
            }

            default:
                throw new InvalidDataException($"Unknown instruction type {GetInstructionType(Kind)} from opcode{Kind}.");
        }
    }

    public static int GetDataTypeStackLength(CodeInstructionType type) {
        return type switch {
            CodeInstructionType.Int16 or CodeInstructionType.Int32 or CodeInstructionType.Float => 4,
            CodeInstructionType.Int64 or CodeInstructionType.Double => 8,
            CodeInstructionType.Variable => 16,
            _ => 16,
        };
    }

    private static byte OldOpcodeToNew(byte opcode) {
        return opcode switch {
            <= 0x10 or 0x41 or 0x82 => (byte)(opcode + 0x04),
            <= 0x16 => 0x15,
            0xC0 or 0xFF => opcode,
            _ => (byte)(opcode - 0x01),
        };
    }

    private static byte NewOpcodeToOld(byte opcode, byte comparison) {
        return opcode switch {
            <= 0x14 or 0x45 or 0x86 => (byte)(opcode - 0x04),
            0x15 => (byte)(0x10 + comparison),
            0x84 => 0xC0,
            0xC0 or 0xFF => opcode,
            >= 0xC1 and <= 0xC3 => 0xC0,
            _ => (byte)(opcode + 0x01),
        };
    }

    public static CodeInstructionMetaKind GetInstructionType(CodeInstructionKind kind) {
        switch (kind) {
            case CodeInstructionKind.Neg:
            case CodeInstructionKind.Not:
            case CodeInstructionKind.Dup:
            case CodeInstructionKind.Ret:
            case CodeInstructionKind.Exit:
            case CodeInstructionKind.PopNull:
            case CodeInstructionKind.CallV:
                return CodeInstructionMetaKind.SingleType;

            case CodeInstructionKind.Conv:
            case CodeInstructionKind.Mul:
            case CodeInstructionKind.Div:
            case CodeInstructionKind.Rem:
            case CodeInstructionKind.Mod:
            case CodeInstructionKind.Add:
            case CodeInstructionKind.Sub:
            case CodeInstructionKind.And:
            case CodeInstructionKind.Or:
            case CodeInstructionKind.Xor:
            case CodeInstructionKind.Shl:
            case CodeInstructionKind.Shr:
                return CodeInstructionMetaKind.DoubleType;

            case CodeInstructionKind.Cmp:
                return CodeInstructionMetaKind.Comparison;

            case CodeInstructionKind.B:
            case CodeInstructionKind.Bt:
            case CodeInstructionKind.Bf:
            case CodeInstructionKind.PushEnv:
            case CodeInstructionKind.PopEnv:
                return CodeInstructionMetaKind.Branch;

            case CodeInstructionKind.Pop:
                return CodeInstructionMetaKind.Pop;

            case CodeInstructionKind.Push:
            case CodeInstructionKind.PushLocal:
            case CodeInstructionKind.PushGlobal:
            case CodeInstructionKind.PushBuiltin:
            case CodeInstructionKind.PushI:
                return CodeInstructionMetaKind.Push;

            case CodeInstructionKind.Call:
                return CodeInstructionMetaKind.Call;

            case CodeInstructionKind.Break:
                return CodeInstructionMetaKind.Break;

            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }
}
