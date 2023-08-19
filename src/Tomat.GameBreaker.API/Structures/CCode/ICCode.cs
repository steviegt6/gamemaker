namespace Tomat.GameBreaker.API.Structures.CCode;

public interface ICCode {
    ICCode Next { get; set; }

    int Kind { get; set; }

    int Compiled { get; set; }

    string? Str { get; set; }

    RToken Token { get; set; }

    RValue Value { get; set; }

    IVmBuffer Vm { get; set; }

    IVmBuffer VmDebugInfo { get; set; }

    string? Code { get; set; }

    string? Name { get; set; }

    int CodeIndex { get; set; }

    IYyGmlFuncs Func { get; set; }

    bool Watch { get; set; }

    int Offset { get; set; }

    int Locals { get; set; }

    int Args { get; set; }

    int Flags { get; set; }

    IYyObjectBase Prototype { get; set; }
}
