namespace Tomat.GameBreaker.API.Marshalling.DataTypes;

public unsafe interface IRValue {
    // union {
    double Real { get; set; }

    int Int32 { get; set; }

    long Int64 { get; set; }

    // union {
    void* Pointer { get; set; }
    // }
    // }

    int Flags { get; set; }

    int Kind { get; set; }
}
