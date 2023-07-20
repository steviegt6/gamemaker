namespace Tomat.GameMaker.IFF.DataTypes.Models.Code; 

public enum GameMakerCodeInstanceType : short {
    // >= 0 are object indices, usually.
    Undefined = 0,
    Self      = -1,
    Other     = -2,
    All       = -3,
    Noone     = -4,
    Global    = -5,
    Builtin   = -6, // Not used in bytecode itself, probably?
    Local     = -7,
    StackTop  = -9,
    Argument  = -15,
    Static    = -16,
}
