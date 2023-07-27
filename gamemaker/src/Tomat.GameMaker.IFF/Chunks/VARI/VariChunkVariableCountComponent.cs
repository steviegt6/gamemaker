namespace Tomat.GameMaker.IFF.Chunks.VARI;

public interface IVariChunkVariableCountComponent {
    int VarCount1 { get; set; }

    int VarCount2 { get; set; }

    int MaxLocalVarCount { get; set; }
}

public class VariChunkVariableCountComponent : IVariChunkVariableCountComponent {
    public required int VarCount1 { get; set; }

    public required int VarCount2 { get; set; }

    public required int MaxLocalVarCount { get; set; }
}
