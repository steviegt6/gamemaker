﻿using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents a context for serializing data to a GameMaker IFF file.
/// </summary>
/// <param name="Writer">
///     The writer used to write to a GameMaker IFF file.
/// </param>
/// <param name="IffFile">The GameMaker IFF file being written to.</param>
/// <param name="VersionInfo">
///     The version information of the GameMaker IFF file being read from.
/// </param>
public sealed record SerializationContext(GameMakerIffWriter Writer, GameMakerIffFile IffFile, GameMakerVersionInfo VersionInfo);

// public static class SerializationContextExtensions { }
