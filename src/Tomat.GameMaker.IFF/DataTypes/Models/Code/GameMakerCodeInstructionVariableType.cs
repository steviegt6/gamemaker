namespace Tomat.GameMaker.IFF.DataTypes.Models.Code;

public enum GameMakerCodeInstructionVariableType : byte {
    Array        = 0x00,
    StackTop     = 0x80,
    Normal       = 0xA0,
    Instance     = 0xE0, // Used for room creation?

    // GMS 2.3 types.
    MultiPush    = 0x10, // Multidimensional array, used with pushaf.
    MultiPushPop = 0x90, // Multidimensional array, used with pushaf/popaf.
}
