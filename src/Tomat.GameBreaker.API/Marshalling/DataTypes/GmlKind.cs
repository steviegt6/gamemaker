namespace Tomat.GameBreaker.API.Marshalling.DataTypes;

public enum GmlKind : uint {
    None   = 0x00000000,
    Double = 0x00000001,
    String = 0x00000002,
    Int32  = 0x00000004,
    Error  = 0xFFFF0000,
}
