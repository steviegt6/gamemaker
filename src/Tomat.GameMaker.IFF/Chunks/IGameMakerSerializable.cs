namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents an arbitrary piece of data that can be serialized to and from
///     a GameMaker IFF file.
/// </summary>
public interface IGameMakerSerializable {
    /// <summary>
    ///     Reads the data from the given context.
    /// </summary>
    /// <param name="context">The context to read from.</param>
    /// <returns>
    ///     The number of bytes read from the context.
    /// </returns>
    void Read(DeserializationContext context);

    /// <summary>
    ///     Writes the data to the given context.
    /// </summary>
    /// <param name="context">The context to write to.</param>
    /// <returns>
    ///     The number of bytes written to the context.
    /// </returns>
    void Write(SerializationContext context);
}
