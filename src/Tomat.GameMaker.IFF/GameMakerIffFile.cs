using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.FORM;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF;

/// <summary>
///     Represents a GameMaker IFF file.
/// </summary>
public sealed class GameMakerIffFile : IGameMakerSerializable {
    /// <summary>
    ///     Gets or sets the FORM chunk of the file. This is the only chunk
    ///     within a GameMaker IFF file, and other chunks are contained within
    ///     this chunk.
    /// </summary>
    public GameMakerFormChunk Form { get; set; } = null!;

    public void Read(DeserializationContext context) {
        var formName = new string(context.ReadChars(IGameMakerChunk.NAME_LENGTH));
        if (formName != GameMakerFormChunk.NAME)
            throw new IOException("IFF file does not start with FORM chunk.");

        var formSize = context.ReadInt32();
        Form = new GameMakerFormChunk(formName, formSize);
        Form.Read(context);
    }

    public void Write(SerializationContext context) {
        if (Form is null)
            throw new IOException("Cannot write IFF file without a FORM chunk.");

        context.Write("FORM"u8.ToArray());
        var formSizePos = context.BeginLength();
        Form.Write(context);
        context.EndLength(formSizePos);

        context.FinalizePointers();
    }

    // TODO: Reader options that lets us specify things like the GameMaker
    // version, for the sake of time optimizations.
    /// <summary>
    ///     Reads a GameMaker IFF file from the given <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <param name="context">The deserialization context.</param>
    /// <returns>
    ///     A <see cref="GameMakerIffFile"/> read from the
    ///     <paramref name="stream"/>.
    /// </returns>
    public static GameMakerIffFile FromStream(Stream stream, out DeserializationContext context) {
        var reader = GameMakerIffReader.FromStream(stream);
        var file = new GameMakerIffFile();
        file.Read(context = new DeserializationContext(reader, file, new GameMakerVersionInfo()));
        return file;
    }
}

public static class GameMakerIffFileExtensions {
    /// <summary>
    ///     Gets all chunks of the given type from the IFF file.
    /// </summary>
    /// <param name="file">The IFF file to get chunks from.</param>
    /// <typeparam name="T">The chunk type.</typeparam>
    /// <returns>A collection of all resolved chunks of the type.</returns>
    public static IEnumerable<T> GetChunks<T>(this GameMakerIffFile file) where T : IGameMakerChunk {
        if (file.Form?.Chunks is null)
            yield break;

        foreach (var chunk in file.Form.Chunks.Values) {
            if (chunk is T tChunk)
                yield return tChunk;
        }
    }

    /// <summary>
    ///     Expects a single chunk from <see cref="GetChunks{T}"/>.
    /// </summary>
    /// <param name="file">The IFF file to get the chunk from.</param>
    /// <typeparam name="T">The chunk type.</typeparam>
    /// <returns>The single expected chunk.</returns>
    public static T GetChunk<T>(this GameMakerIffFile file) where T : IGameMakerChunk {
        return file.GetChunks<T>().Single();
    }
}
