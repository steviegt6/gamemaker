namespace Tomat.GameMaker.IFF.DataTypes.Models.Code; 

public enum GameMakerInstructionDataType : byte {
    Double,
    Float,
    Int32,
    Int64,
    Boolean,
    Variable,
    String,
    Instance,
    
    // These four seem used.
    Delete,
    Undefined,
    UnsignedInt,
    Int16 = 0x0F,
    
    Unset = 0xFF,
}
