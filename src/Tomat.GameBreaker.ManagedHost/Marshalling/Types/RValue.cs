using System.Runtime.InteropServices;
using Tomat.GameBreaker.API.Marshalling;
using Tomat.GameBreaker.API.Marshalling.DataTypes;

namespace Tomat.GameBreaker.ManagedHost.Marshalling.Types;

// We don't need to worry about pointer size differences since the inclusion of
// a long (long long) in the struct forces the union to be reliably 8 bytes.
[StructLayout(LayoutKind.Explicit, Size = sizeof(long) + (sizeof(int) * 2))]
internal unsafe struct RValue : IRValue {
    [field: FieldOffset(0)]
    public double Real { get; set; }

    [field: FieldOffset(0)]

    public int Int32 { get; set; }

    [field: FieldOffset(0)]
    public long Int64 { get; set; }

    [field: FieldOffset(0)]
    public void* Pointer { get; set; }

    [field: FieldOffset(sizeof(long))]
    public int Flags { get; set; }

    [field: FieldOffset(sizeof(long) + sizeof(int))]
    public int Kind { get; set; }
}

internal sealed class RValueMarshaller : IMarshaller<IRValue> {
    public IRValue FromPtr(nint ptr) {
        return Marshal.PtrToStructure<RValue>(ptr);
    }

    public unsafe nint ToPtr(IRValue source) {
        var ptr = Marshal.AllocHGlobal(sizeof(RValue));
        Marshal.StructureToPtr(source, ptr, false);
        return ptr;
    }
}
