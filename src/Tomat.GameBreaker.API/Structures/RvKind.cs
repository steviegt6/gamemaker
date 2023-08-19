namespace Tomat.GameBreaker.API.Structures;

public enum RvKind {
    /// <summary>
    ///     Real value.
    /// </summary>
    Real,

    /// <summary>
    ///     String value.
    /// </summary>
    String,

    /// <summary>
    ///     Array value.
    /// </summary>
    Array,

    /// <summary>
    ///     Pointer value.
    /// </summary>
    Pointer,

    /// <summary>
    ///     Vector3 (x, y, z) value.
    /// </summary>
    Vector3,

    /// <summary>
    ///     Undefined value.
    /// </summary>
    Undefined,

    /// <summary>
    ///     <c>YYObjectBase*</c> value.
    /// </summary>
    Object,

    /// <summary>
    ///     32-bit integer value.
    /// </summary>
    Int32,

    /// <summary>
    ///     Vector4 (x, y, z, w) value.
    /// </summary>
    Vector4,

    /// <summary>
    ///     "Vector44" (4x4 matrix) value.
    /// </summary>
    Matrix4X4,

    /// <summary>
    ///     64-bit integer value.
    /// </summary>
    Int64,

    /// <summary>
    ///     An accessor.
    /// </summary>
    Accessor,

    /// <summary>
    ///     JavaScript null value.
    /// </summary>
    Null,

    /// <summary>
    ///     Boolean value.
    /// </summary>
    Bool,

    /// <summary>
    ///     JavaScript for-in iterator.
    /// </summary>
    Iterator,

    /// <summary>
    ///     Reference value.
    /// </summary>
    Ref,

    Unset = 0x0FFFFFF,
}
