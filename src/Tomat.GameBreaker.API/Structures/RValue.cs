using System.Runtime.InteropServices;

namespace Tomat.GameBreaker.API.Structures; 

public unsafe struct RValue {
    [FieldOffset(0)]
    private double real;
    
    [FieldOffset(0)]
    private int i32;
    
    [FieldOffset(0)]
    private long i64;
    
    [FieldOffset(0)]
    private YyObjectBase* @object;
    
    [FieldOffset(nint.Size)]
    private CInstance* instance;
    private RefString* @string;
    private CDynamicArrayRef<RValue> embeddedArray;
    private RefDynamicArrayOfRValue* refArray;
    private void* pointer;
}
