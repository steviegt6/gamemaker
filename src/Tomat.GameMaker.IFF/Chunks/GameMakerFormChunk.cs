using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.Chunks; 

/// <summary>
///     The FORM chunk making up the entire GameMaker IFF file. This chunk may
///     contain many smaller chunks, as it contains its own IFF file structure.
/// </summary>
public sealed class GameMakerFormChunk : IGameMakerChunk {
    public const string NAME = "FORM";
    
    public string? Name { get; set; }

    public int Size { get; set; }
    
    public int Read(DeserializationContext context) {
        throw new System.NotImplementedException();
    }

    public int Write(SerializationContext context) {
        throw new System.NotImplementedException();
    }
}
