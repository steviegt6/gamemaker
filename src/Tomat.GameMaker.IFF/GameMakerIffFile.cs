using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;
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
    public GameMakerFormChunk? Form { get; set; }

    public int Read(DeserializationContext context) {
        Form = new GameMakerFormChunk();
        return Form.Read(context);
    }

    public int Write(SerializationContext context) {
        if (Form is null)
            throw new IOException("Cannot write IFF file without a FORM chunk.");

        return Form.Write(context);
    }

    // TODO: Reader options that lets us specify things like the GameMaker
    // version, for the sake of time optimizations.
    /// <summary>
    ///     Reads a GameMaker IFF file from the given <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <returns>
    ///     A <see cref="GameMakerIffFile"/> read from the
    ///     <paramref name="stream"/>.
    /// </returns>
    public static GameMakerIffFile FromStream(Stream stream) {
        var reader = GameMakerIffReader.FromStream(stream);
        var file = new GameMakerIffFile();
        file.Read(new DeserializationContext(reader, file));
        return file;
    }
}
