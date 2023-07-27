namespace Tomat.GameMaker.IFF.Chunks.GEN8;

public interface IGen8ChunkDebuggerPortComponent {
    int DebuggerPort { get; set; }
}

internal sealed class Gen8ChunkDebuggerPortComponent : IGen8ChunkDebuggerPortComponent {
    public required int DebuggerPort { get; set; }
}
