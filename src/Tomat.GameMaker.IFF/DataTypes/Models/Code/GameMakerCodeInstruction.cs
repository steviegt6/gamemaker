using System;
using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Code;

public sealed class GameMakerCodeInstruction : IGameMakerSerializable {
    public int Address { get; set; }

    public GameMakerCodeInstructionOpcode Opcode { get; set; }

    public GameMakerCodeComparisonType ComparisonType { get; set; }

    public GameMakerInstructionDataType DataType1 { get; set; }

    public GameMakerInstructionDataType DataType2 { get; set; }

    public GameMakerCodeInstanceType InstanceType { get; set; }

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

            if (GetInstructionType(Opcode) == GameMakerCodeInstructionType.Push) {
                if (DataType1 is GameMakerInstructionDataType.Double or GameMakerInstructionDataType.Int64)
                    return 3;

                if (DataType1 != GameMakerInstructionDataType.Int16)
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
        Opcode = (GameMakerCodeInstructionOpcode)opcode;
        context.Position = start;

        switch (GetInstructionType(Opcode)) {
            case GameMakerCodeInstructionType.SingleType:
            case GameMakerCodeInstructionType.DoubleType:
            case GameMakerCodeInstructionType.Comparison: {
                Extra = context.ReadByte();

                if (Extra != 0 && Opcode != GameMakerCodeInstructionOpcode.Dup && Opcode != GameMakerCodeInstructionOpcode.CallV)
                    throw new InvalidDataException("Expected extra byte of 0 for opcode " + Opcode + ", got " + Extra + ".");

                ComparisonType = (GameMakerCodeComparisonType)context.ReadByte();

                var types = context.ReadByte();
                DataType1 = (GameMakerInstructionDataType)(types & 0xF);
                DataType2 = (GameMakerInstructionDataType)(types >> 4);
                if (Opcode == GameMakerCodeInstructionOpcode.Cmp && context.VersionInfo.FormatId <= 14)
                    ComparisonType = (GameMakerCodeComparisonType)(context.ReadByte() - 0x10);
                else
                    context.Position += 1;

                if (Opcode is GameMakerCodeInstructionOpcode.And or GameMakerCodeInstructionOpcode.Or) {
                    if (DataType1 == GameMakerInstructionDataType.Boolean && DataType2 == GameMakerInstructionDataType.Boolean)
                        context.VersionInfo.ShortCircuit = false;
                }

                break;
            }

            case GameMakerCodeInstructionType.Branch: {
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

            case GameMakerCodeInstructionType.Pop: {
                InstanceType = (GameMakerCodeInstanceType)context.ReadInt16();

                var types = context.ReadByte();
                DataType1 = (GameMakerInstructionDataType)(types & 0xF);
                DataType2 = (GameMakerInstructionDataType)(types >> 4);

                context.Position += 1;

                // Ignore swap instructions.
                if (DataType1 != GameMakerInstructionDataType.Int16) {
                    Variable = new GameMakerCodeInstructionReference<GameMakerVariable>();
                    Variable.Read(context);
                }

                break;
            }

            case GameMakerCodeInstructionType.Push: {
                var val = context.ReadInt16();

                DataType1 = (GameMakerInstructionDataType)context.ReadByte();

                if (context.VersionInfo.FormatId <= 14) {
                    if (DataType1 == GameMakerInstructionDataType.Variable) {
                        Opcode = val switch {
                            -5 => GameMakerCodeInstructionOpcode.PushGlb,
                            -6 => GameMakerCodeInstructionOpcode.PushBltn,
                            -7 => GameMakerCodeInstructionOpcode.PushLoc,
                            _ => Opcode,
                        };
                    }
                    else if (DataType1 == GameMakerInstructionDataType.Int16) {
                        Opcode = GameMakerCodeInstructionOpcode.PushI;
                    }
                }

                context.Position += 1;

                switch (DataType1) {
                    case GameMakerInstructionDataType.Double:
                        Value = context.ReadDouble();
                        break;

                    case GameMakerInstructionDataType.Float:
                        Value = context.ReadSingle();
                        break;

                    case GameMakerInstructionDataType.Int32:
                        Value = context.ReadInt32();
                        break;

                    case GameMakerInstructionDataType.Int64:
                        Value = context.ReadInt64();
                        break;

                    case GameMakerInstructionDataType.Boolean:
                        Value = context.ReadBoolean(wide: true);
                        break;

                    case GameMakerInstructionDataType.Variable:
                        InstanceType = (GameMakerCodeInstanceType)val;
                        Variable = new GameMakerCodeInstructionReference<GameMakerVariable>();
                        Variable.Read(context);
                        break;

                    case GameMakerInstructionDataType.String:
                        Value = context.ReadInt32(); // String ID.
                        break;

                    case GameMakerInstructionDataType.Int16:
                        Value = val;
                        break;
                }

                break;
            }

            case GameMakerCodeInstructionType.Call: {
                Value = context.ReadInt16();
                DataType1 = (GameMakerInstructionDataType)context.ReadByte();

                context.Position += 1;

                Function = new GameMakerCodeInstructionReference<GameMakerFunctionEntry>();
                Function.Read(context);
                break;
            }

            case GameMakerCodeInstructionType.Break: {
                Value = context.ReadUInt16();
                DataType1 = (GameMakerInstructionDataType)context.ReadByte();

                context.Position += 1;
                break;
            }

            default:
                throw new InvalidDataException($"Unknown instruction type {GetInstructionType(Opcode)} from opcode{Opcode}.");
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

        switch (GetInstructionType(Opcode)) {
            case GameMakerCodeInstructionType.SingleType:
            case GameMakerCodeInstructionType.DoubleType:
            case GameMakerCodeInstructionType.Comparison: {
                context.Write(Extra);
                if (context.VersionInfo.FormatId <= 14 && Opcode == GameMakerCodeInstructionOpcode.Cmp)
                    context.Write((byte)0);
                else
                    context.Write((byte)ComparisonType);
                context.Write((byte)((byte)DataType2 << 4 | (byte)DataType1));
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Opcode, (byte)ComparisonType));
                else
                    context.Write((byte)Opcode);
                break;
            }

            case GameMakerCodeInstructionType.Branch: {
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(JumpOffset);
                else if (PopenvExitMagic)
                    context.Write((Int24)0xF00000);
                else
                    context.Write((Int24)(int)((uint)(int)JumpOffset & ~0xFF800000));

                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Opcode, 0));
                else
                    context.Write((byte)Opcode);
                break;
            }

            case GameMakerCodeInstructionType.Pop: {
                context.Write((short)InstanceType);
                context.Write((byte)((byte)DataType2 << 4 | (byte)DataType1));
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Opcode, 0));
                else
                    context.Write((byte)Opcode);

                if (DataType1 != GameMakerInstructionDataType.Int16) {
                    if (Variable is null)
                        throw new InvalidOperationException($"Missing variable at {context.Position}.");

                    Variable.Write(context);
                }

                break;
            }

            case GameMakerCodeInstructionType.Push: {
                if (DataType1 == GameMakerInstructionDataType.Int16) {
                    if (Value is not short value)
                        throw new InvalidOperationException($"Missing value at {context.Position}.");

                    context.Write(value);
                }
                else if (DataType1 == GameMakerInstructionDataType.Variable) {
                    context.Write((short)InstanceType);
                }
                else {
                    context.Write((short)0);
                }

                context.Write((byte)DataType1);

                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Opcode, 0));
                else
                    context.Write((byte)Opcode);

                switch (DataType1) {
                    case GameMakerInstructionDataType.Double: {
                        if (Value is not double value)
                            throw new InvalidOperationException($"Missing value at {context.Position}.");

                        context.Write(value);
                        break;
                    }

                    case GameMakerInstructionDataType.Float: {
                        if (Value is not float value)
                            throw new InvalidOperationException($"Missing value at {context.Position}.");

                        context.Write(value);
                        break;
                    }

                    case GameMakerInstructionDataType.Int32: {
                        if (Value is not int value)
                            throw new InvalidOperationException($"Missing value at {context.Position}.");

                        context.Write(value);
                        break;
                    }

                    case GameMakerInstructionDataType.Int64: {
                        if (Value is not long value)
                            throw new InvalidOperationException($"Missing value at {context.Position}.");

                        context.Write(value);
                        break;
                    }

                    case GameMakerInstructionDataType.Boolean: {
                        if (Value is not bool value)
                            throw new InvalidOperationException($"Missing value at {context.Position}.");

                        context.Write(value, wide: true);
                        break;
                    }

                    case GameMakerInstructionDataType.Variable: {
                        if (Variable is null)
                            throw new InvalidOperationException($"Missing variable at {context.Position}.");

                        Variable.Write(context);
                        break;
                    }

                    case GameMakerInstructionDataType.String: {
                        if (Value is not int value)
                            throw new InvalidOperationException($"Missing value at {context.Position}.");

                        context.Write(value); // String ID.
                        break;
                    }

                    // case GameMakerInstructionDataType.Int16:
                    //     break;
                }

                break;
            }

            case GameMakerCodeInstructionType.Call: {
                if (Value is not short value)
                    throw new InvalidOperationException($"Missing value at {context.Position}.");

                context.Write(value);
                context.Write((byte)DataType1);
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Opcode, 0));
                else
                    context.Write((byte)Opcode);

                if (Function is null)
                    throw new InvalidOperationException($"Missing function at {context.Position}.");

                Function.Write(context);
                break;
            }

            case GameMakerCodeInstructionType.Break: {
                if (Value is not ushort value)
                    throw new InvalidOperationException($"Missing value at {context.Position}.");

                context.Write(value);
                context.Write((byte)DataType1);
                if (context.VersionInfo.FormatId <= 14)
                    context.Write(NewOpcodeToOld((byte)Opcode, 0));
                else
                    context.Write((byte)Opcode);
                break;
            }

            default:
                throw new InvalidDataException($"Unknown instruction type {GetInstructionType(Opcode)} from opcode{Opcode}.");
        }
    }

    public static int GetDataTypeStackLength(GameMakerInstructionDataType dataType) {
        return dataType switch {
            GameMakerInstructionDataType.Int16 or GameMakerInstructionDataType.Int32 or GameMakerInstructionDataType.Float => 4,
            GameMakerInstructionDataType.Int64 or GameMakerInstructionDataType.Double => 8,
            GameMakerInstructionDataType.Variable => 16,
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

    public static GameMakerCodeInstructionType GetInstructionType(GameMakerCodeInstructionOpcode opcode) {
        switch (opcode) {
            case GameMakerCodeInstructionOpcode.Neg:
            case GameMakerCodeInstructionOpcode.Not:
            case GameMakerCodeInstructionOpcode.Dup:
            case GameMakerCodeInstructionOpcode.Ret:
            case GameMakerCodeInstructionOpcode.Exit:
            case GameMakerCodeInstructionOpcode.Popz:
            case GameMakerCodeInstructionOpcode.CallV:
                return GameMakerCodeInstructionType.SingleType;

            case GameMakerCodeInstructionOpcode.Conv:
            case GameMakerCodeInstructionOpcode.Mul:
            case GameMakerCodeInstructionOpcode.Div:
            case GameMakerCodeInstructionOpcode.Rem:
            case GameMakerCodeInstructionOpcode.Mod:
            case GameMakerCodeInstructionOpcode.Add:
            case GameMakerCodeInstructionOpcode.Sub:
            case GameMakerCodeInstructionOpcode.And:
            case GameMakerCodeInstructionOpcode.Or:
            case GameMakerCodeInstructionOpcode.Xor:
            case GameMakerCodeInstructionOpcode.Shl:
            case GameMakerCodeInstructionOpcode.Shr:
                return GameMakerCodeInstructionType.DoubleType;

            case GameMakerCodeInstructionOpcode.Cmp:
                return GameMakerCodeInstructionType.Comparison;

            case GameMakerCodeInstructionOpcode.B:
            case GameMakerCodeInstructionOpcode.Bt:
            case GameMakerCodeInstructionOpcode.Bf:
            case GameMakerCodeInstructionOpcode.PushEnv:
            case GameMakerCodeInstructionOpcode.PopEnv:
                return GameMakerCodeInstructionType.Branch;

            case GameMakerCodeInstructionOpcode.Pop:
                return GameMakerCodeInstructionType.Pop;

            case GameMakerCodeInstructionOpcode.Push:
            case GameMakerCodeInstructionOpcode.PushLoc:
            case GameMakerCodeInstructionOpcode.PushGlb:
            case GameMakerCodeInstructionOpcode.PushBltn:
            case GameMakerCodeInstructionOpcode.PushI:
                return GameMakerCodeInstructionType.Push;

            case GameMakerCodeInstructionOpcode.Call:
                return GameMakerCodeInstructionType.Call;

            case GameMakerCodeInstructionOpcode.Break:
                return GameMakerCodeInstructionType.Break;

            default:
                throw new ArgumentOutOfRangeException(nameof(opcode), opcode, null);
        }
    }
}
