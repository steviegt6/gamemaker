namespace Tomat.GameBreaker.API.Marshalling.DataTypes;

public enum RvKind {
    Real      = 0,
    String    = 1,
    Array     = 2,
    Pointer   = 3,
    Vector3   = 4,
    Undefined = 5,
    Object    = 6,
    Int32     = 7,
    Vector4   = 8,
    Matrix4X4 = 9,
    Int64     = 10,
    Accessor  = 11,
    Null      = 12,
    Bool      = 13,
    Iterator  = 14,
    Ref       = 15,
    Unset     = 0x0FFFFFF,
}
