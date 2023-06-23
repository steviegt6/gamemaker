using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.Contexts;

/// <summary>
///     Represents a context for serializing data to a GameMaker IFF file.
/// </summary>
/// <param name="Writer">
///     The writer used to write to a GameMaker IFF file.
/// </param>
/// <param name="IffFile">The GameMaker IFF file being written to.</param>
public sealed record SerializationContext(GameMakerIffWriter Writer, GameMakerIffFile IffFile);
