using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.DataTypes; 

/// <summary>
///     Represents a string within a GameMaker IFF file.
/// </summary>
public sealed class GameMakerString : IGameMakerSerializable {
    /// <summary>
    ///     The value of the string.
    /// </summary>
    public string? Value { get; set; }

    public int Read(DeserializationContext context) {
        throw new System.NotImplementedException();
    }

    public int Write(SerializationContext context) {
        throw new System.NotImplementedException();
    }
}
