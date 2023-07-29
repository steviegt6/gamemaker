namespace Tomat.GameMaker.IFF.Chunks.GEN8;

/// <summary>
///     Contains the debugger port if the bytecode version is 14 or greater.
/// </summary>
public interface IGen8ChunkDebuggerPortComponent {
    /// <summary>
    ///     The debugger port.
    /// </summary>
    int DebuggerPort { get; set; }
}

internal sealed class Gen8ChunkDebuggerPortComponent : IGen8ChunkDebuggerPortComponent {
    public required int DebuggerPort { get; set; }
}
