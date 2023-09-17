namespace Tomat.GameMaker.IFF.Chunks.DFNC;

public interface IDfncChunk {
    int ChunkVersion { get; set; }

    GameMakerPointerList<GameMakerDebugFunctionInfo> Functions { get; set; }
}

internal sealed class GameMakerDfncChunk : AbstractChunk,
                                           IDfncChunk {
    public const string NAME = "DFNC";

    public int ChunkVersion { get; set; }

    public GameMakerPointerList<GameMakerDebugFunctionInfo> Functions { get; set; } = null!;

    public GameMakerDfncChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        ChunkVersion = context.ReadInt32();
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        Functions = context.ReadPointerList<GameMakerDebugFunctionInfo>();
    }

    public override void Write(SerializationContext context) {
        if (ChunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.");

        context.Write(ChunkVersion);
        context.Write(Functions);
    }
}
