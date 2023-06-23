using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.Contexts;

/// <summary>
///     Represents a context for serializing data to a GameMaker IFF file.
/// </summary>
/// <param name="Reader">
///     The reader used to read from a GameMaker IFF file.
/// </param>
/// <param name="IffFile">The GameMaker IFF file being read from.</param>
public sealed record DeserializationContext(GameMakerIffReader Reader, GameMakerIffFile IffFile);
